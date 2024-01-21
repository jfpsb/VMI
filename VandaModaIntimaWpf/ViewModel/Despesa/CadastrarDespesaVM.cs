using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;

namespace VandaModaIntimaWpf.ViewModel.Despesa
{
    public class CadastrarDespesaVM : ACadastrarViewModel<Model.Despesa>
    {
        private bool _isOutrosDespesas;
        private string _tipoDescricao;
        private DAOTipoDespesa daoTipoDespesa;
        private DAOFornecedor daoFornecedor;
        private DAOLoja daoLoja;
        private DAO<MembroFamiliar> daoMembroFamiliar;
        private DAORepresentante daoRepresentante;
        private Visibility _visibilidadeDespesaEmpresarial;
        private Visibility _visibilidadeMembroFamiliar;
        private bool _inserirVencimentoFlag;
        private string[] _tiposDescricao = GetResource.GetStringArray("CmbTipoDescricaoDespesa");
        private bool _isCmbLojasEnabled = true;
        private IList<EntidadeComCampo<Model.Loja>> lojasEntidadeComCampo;
        private bool _registrarCompraDeFornecedor;
        private bool _podeRegistrarCompraDeFornecedor;

        protected DateTime? ultimaDataVencimento = null;

        private ObservableCollection<Model.TipoDespesa> _tiposDespesa;
        private ObservableCollection<Model.Fornecedor> _fornecedores;
        private ObservableCollection<Model.Representante> _representantes;
        private ObservableCollection<Model.Loja> _lojas;
        private ObservableCollection<Model.MembroFamiliar> _membrosFamiliar;

        public ICommand SelecionarLojasComando { get; set; }

        public CadastrarDespesaVM(ISession session, bool isUpdate = false) : base(session, isUpdate)
        {
            viewModelStrategy = new CadastrarDespesaVMStrategy();
            daoEntidade = new DAO<Model.Despesa>(_session);
            daoTipoDespesa = new DAOTipoDespesa(_session);
            daoFornecedor = new DAOFornecedor(_session);
            daoRepresentante = new DAORepresentante(_session);
            daoLoja = new DAOLoja(_session);
            daoMembroFamiliar = new DAO<MembroFamiliar>(_session);

            Entidade = new Model.Despesa()
            {
                Data = DateTime.Now
            };

            GetFornecedores();
            GetTiposDespesa();
            GetRepresentantes();
            GetLojas();
            GetMembrosFamiliar();
            Entidade.Descricao = TipoDescricao = "CONTA DE LUZ"; //Primeiro item

            SelecionarLojasComando = new RelayCommand(SelecionarLojas);

            PropertyChanged += CadastrarDespesaVM_PropertyChanged;
        }

        private void SelecionarLojas(object obj)
        {
            _windowService.ShowDialog(new SelecionaMultiplasLojasVM(_session, lojasEntidadeComCampo), (result, viewModel) =>
            {
                var vm = viewModel as SelecionaMultiplasLojasVM;
                if (!vm.Entidades.Where(w => w.IsChecked).Any())
                {
                    IsCmbLojasEnabled = true;
                    lojasEntidadeComCampo?.Clear();
                }
                else
                {
                    IsCmbLojasEnabled = false;
                    lojasEntidadeComCampo = vm.Entidades;
                }
            });
        }

        protected async override Task<AposCRUDEventArgs> ExecutarSalvar(object parametro)
        {
            _result = false;
            try
            {
                //Caso seja cadastro de mesma despesa para várias lojas
                if (lojasEntidadeComCampo != null && lojasEntidadeComCampo.Count > 0)
                {
                    string mensagem = "Múltiplas lojas estão selecionadas. Esta despesa será cadastrada na(s) loja(s):\n";
                    var lojasMarcadas = lojasEntidadeComCampo.Where(w => w.IsChecked);

                    foreach (var lojaMarcada in lojasMarcadas)
                    {
                        mensagem += $"{lojaMarcada.Entidade.Nome}\n";
                    }

                    mensagem += "\nConfirma?";

                    var result = _messageBoxService.Show(mensagem,
                        viewModelStrategy.MessageBoxCaption(),
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question,
                        MessageBoxResult.No);

                    if (result == MessageBoxResult.Yes)
                    {
                        IList<Model.Despesa> despesas = new List<Model.Despesa>();
                        IList<Model.CompraDeFornecedor> compras = new List<Model.CompraDeFornecedor>();

                        foreach (var lojaComCampo in lojasMarcadas)
                        {
                            var loja = lojaComCampo.Entidade;
                            var despesa = new Model.Despesa()
                            {
                                TipoDespesa = Entidade.TipoDespesa,
                                Adiantamento = Entidade.Adiantamento,
                                Fornecedor = Entidade.Fornecedor,
                                Representante = Entidade.Representante,
                                Loja = loja,
                                Familiar = Entidade.Familiar,
                                Data = Entidade.Data,
                                DataVencimento = Entidade.DataVencimento,
                                Descricao = Entidade.Descricao,
                                Valor = Entidade.Valor,
                                Detalhes = Entidade.Detalhes
                            };

                            despesas.Add(despesa);

                            if (RegistrarCompraDeFornecedor)
                            {
                                var compra = new Model.CompraDeFornecedor()
                                {
                                    Fornecedor = Entidade.Fornecedor,
                                    Representante = Entidade.Representante,
                                    Loja = loja,
                                    Valor = Entidade.Valor,
                                    DataPedido = Entidade.Data,
                                    Pago = true
                                };

                                compras.Add(compra);
                            }
                        }

                        if (RegistrarCompraDeFornecedor)
                        {
                            await daoEntidade.InserirMultiplasListas(despesas, compras);
                        }
                        else
                        {
                            await daoEntidade.Inserir(despesas);
                        }

                        _result = true;
                        _messageBoxService.Show("Despesas foram salvas com sucesso.",
                            viewModelStrategy.MessageBoxCaption(),
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

                        AposCRUDEventArgs e = new AposCRUDEventArgs()
                        {
                            IssoEhUpdate = IssoEUmUpdate,
                            Sucesso = _result,
                            Parametro = parametro
                        };

                        return e;
                    }
                }
                else if (RegistrarCompraDeFornecedor)
                {
                    var compra = new Model.CompraDeFornecedor()
                    {
                        Fornecedor = Entidade.Fornecedor,
                        Representante = Entidade.Representante,
                        Loja = Entidade.Loja,
                        Valor = Entidade.Valor,
                        DataPedido = Entidade.Data,
                        Pago = true
                    };

                    await daoEntidade.InserirMultiplasEntidades(Entidade, compra);

                    _result = true;

                    _messageBoxService.Show(viewModelStrategy.MensagemEntidadeSalvaComSucesso(), viewModelStrategy.MessageBoxCaption(),
                    MessageBoxButton.OK, MessageBoxImage.Information);

                    AposCRUDEventArgs e = new AposCRUDEventArgs()
                    {
                        IssoEhUpdate = IssoEUmUpdate,
                        Sucesso = _result,
                        Parametro = parametro
                    };

                    return e;
                }                
            }
            catch (Exception ex)
            {
                _messageBoxService.Show($"Erro ao salvar despesas.\n\n{ex.Message}\n\n{ex.InnerException?.Message}", viewModelStrategy.MessageBoxCaption(),
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //Caso as condições acima não sejam verdadeiras, é feito o cadastro com o método base
            return await base.ExecutarSalvar(parametro);
        }

        private void CadastrarDespesaVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TipoDescricao":
                    if (!_tiposDescricao.Contains(TipoDescricao))
                    {
                        TipoDescricao = "OUTROS";
                        break;
                    }

                    if (TipoDescricao.Equals("OUTROS"))
                    {
                        IsOutrosDespesas = true;
                        if (!IssoEUmUpdate)
                        {
                            Entidade.Descricao = string.Empty;
                        }
                    }
                    else
                    {
                        IsOutrosDespesas = false;
                        Entidade.Descricao = TipoDescricao;
                    }
                    break;
                case "InserirVencimentoFlag":
                    if (InserirVencimentoFlag)
                    {
                        if (ultimaDataVencimento == null)
                        {
                            Entidade.DataVencimento = ultimaDataVencimento = DateTime.Today;
                        }
                        else
                        {
                            Entidade.DataVencimento = ultimaDataVencimento;
                        }
                    }
                    else
                    {
                        Entidade.DataVencimento = null;
                    }
                    break;
            }
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TipoDespesa":
                    if (Entidade.TipoDespesa.Nome.Equals("DESPESA EMPRESARIAL"))
                    {
                        VisibilidadeDespesaEmpresarial = Visibility.Visible;
                        VisibilidadeMembroFamiliar = Visibility.Collapsed;

                        Entidade.Familiar = null;

                        if (!IssoEUmUpdate)
                        {
                            Entidade.Fornecedor = Fornecedores[0];
                        }
                    }
                    else if (Entidade.TipoDespesa.Nome.Equals("DESPESA FAMILIAR"))
                    {
                        VisibilidadeDespesaEmpresarial = Visibility.Collapsed;
                        VisibilidadeMembroFamiliar = Visibility.Visible;

                        if (!IssoEUmUpdate)
                        {
                            Entidade.Fornecedor = null;
                            Entidade.Familiar = MembrosFamiliar[0];
                            OnPropertyChanged("TipoDescricao");
                        }
                    }
                    else
                    {
                        VisibilidadeDespesaEmpresarial = Visibility.Collapsed;
                        VisibilidadeMembroFamiliar = Visibility.Collapsed;

                        Entidade.Familiar = null;
                        Entidade.Fornecedor = null;
                        Entidade.Loja = null;
                    }
                    break;
                case "Fornecedor":
                    if (Entidade.Fornecedor != null)
                    {
                        if (Entidade.Fornecedor.Cnpj == null)
                        {
                            Entidade.Fornecedor = null;
                            break;
                        }

                        if (Entidade.Fornecedor.Representante != null)
                        {
                            Entidade.Representante = Entidade.Fornecedor.Representante;
                        }
                    }

                    //Se Fornecedor for null e Representante for null não é possível registrar a despesa como compra de fornecedor
                    if (Entidade.Fornecedor == null && Entidade.Representante == null)
                    {
                        RegistrarCompraDeFornecedor = false;
                        PodeRegistrarCompraDeFornecedor = false;
                    }
                    else if (Entidade.Fornecedor != null && Entidade.Fornecedor.Cnpj == null && Entidade.Representante != null && Entidade.Representante.Id == 0)
                    {
                        RegistrarCompraDeFornecedor = false;
                        PodeRegistrarCompraDeFornecedor = false;
                    }
                    else
                    {
                        PodeRegistrarCompraDeFornecedor = true;
                    }
                    break;
                case "Loja":
                    if (Entidade.Loja != null)
                    {
                        if (Entidade.Loja.Cnpj == null)
                        {
                            Entidade.Loja = null;
                            break;
                        }
                    }
                    break;
                case "Representante":
                    if (Entidade.Representante != null && Entidade.Representante.Id == 0)
                        Entidade.Representante = null;

                    //Se Fornecedor for null e Representante for null não é possível registrar a despesa como compra de fornecedor
                    if (Entidade.Fornecedor == null && Entidade.Representante == null)
                    {
                        RegistrarCompraDeFornecedor = false;
                        PodeRegistrarCompraDeFornecedor = false;
                    }
                    else if (Entidade.Fornecedor != null && Entidade.Fornecedor.Cnpj == null && Entidade.Representante != null && Entidade.Representante.Id == 0)
                    {
                        RegistrarCompraDeFornecedor = false;
                        PodeRegistrarCompraDeFornecedor = false;
                    }
                    else
                    {
                        PodeRegistrarCompraDeFornecedor = true;
                    }
                    break;
            }
        }

        public override void ResetaPropriedades(AposCRUDEventArgs e)
        {
            Model.Despesa novaEntidade = new Model.Despesa();
            novaEntidade.TipoDespesa = Entidade.TipoDespesa;
            novaEntidade.Fornecedor = Entidade.Fornecedor;
            novaEntidade.Loja = Entidade.Loja;
            novaEntidade.Data = Entidade.Data;
            novaEntidade.Descricao = Entidade.Descricao;
            OnPropertyChanged("TipoDescricao");
            if (InserirVencimentoFlag)
                novaEntidade.DataVencimento = Entidade.DataVencimento;

            Entidade = novaEntidade;
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (Entidade.Valor <= 0.0)
            {
                BtnSalvarToolTip += "Informe Um Valor Válido Para A Despesa!\n";
                valido = false;
            }

            if (Entidade.Descricao == null || Entidade.Descricao.Trim().Equals(string.Empty))
            {
                BtnSalvarToolTip += "Informe Uma Descrição Válida Para A Despesa!\n";
                valido = false;
            }

            return valido;
        }
        private async void GetTiposDespesa()
        {
            TiposDespesa = new ObservableCollection<Model.TipoDespesa>(await daoTipoDespesa.Listar());
            Entidade.TipoDespesa = TiposDespesa[0];
        }

        private async void GetFornecedores()
        {
            Fornecedores = new ObservableCollection<Model.Fornecedor>(await daoFornecedor.Listar());
            Fornecedores.Insert(0, new Model.Fornecedor { Nome = "SEM FORNECEDOR" });
            Entidade.Fornecedor = Fornecedores[0];
        }

        private async void GetRepresentantes()
        {
            Representantes = new ObservableCollection<Model.Representante>(await daoRepresentante.Listar());
            Representantes.Insert(0, new Model.Representante { Nome = "SEM REPRESENTANTE" });
            Entidade.Representante = Representantes[0];
        }

        private async void GetLojas()
        {
            Lojas = new ObservableCollection<Model.Loja>(await daoLoja.ListarExcetoDeposito());
            Lojas.Insert(0, new Model.Loja { Nome = "SELECIONE UMA LOJA" });
            Entidade.Loja = Lojas[0];
        }

        private async void GetMembrosFamiliar()
        {
            MembrosFamiliar = new ObservableCollection<Model.MembroFamiliar>(await daoMembroFamiliar.Listar());
        }

        public ObservableCollection<Model.TipoDespesa> TiposDespesa
        {
            get => _tiposDespesa;
            set
            {
                _tiposDespesa = value;
                OnPropertyChanged("TiposDespesa");
            }
        }
        public ObservableCollection<Model.Fornecedor> Fornecedores
        {
            get => _fornecedores;
            set
            {
                _fornecedores = value;
                OnPropertyChanged("Fornecedores");
            }
        }
        public bool IsOutrosDespesas
        {
            get => _isOutrosDespesas;
            set
            {
                _isOutrosDespesas = value;
                OnPropertyChanged("IsOutrosDespesas");
            }
        }
        public string TipoDescricao
        {
            get => _tipoDescricao;
            set
            {
                _tipoDescricao = value;
                OnPropertyChanged("TipoDescricao");
            }
        }

        public Visibility VisibilidadeDespesaEmpresarial
        {
            get => _visibilidadeDespesaEmpresarial;
            set
            {
                _visibilidadeDespesaEmpresarial = value;
                OnPropertyChanged("VisibilidadeDespesaEmpresarial");
            }
        }
        public Visibility VisibilidadeMembroFamiliar
        {
            get => _visibilidadeMembroFamiliar;
            set
            {
                _visibilidadeMembroFamiliar = value;
                OnPropertyChanged("VisibilidadeMembroFamiliar");
            }
        }

        public ObservableCollection<Model.Representante> Representantes
        {
            get => _representantes;
            set
            {
                _representantes = value;
                OnPropertyChanged("Representantes");
            }
        }
        public ObservableCollection<Model.Loja> Lojas
        {
            get => _lojas;
            set
            {
                _lojas = value;
                OnPropertyChanged("Lojas");
            }
        }

        public bool InserirVencimentoFlag
        {
            get => _inserirVencimentoFlag;
            set
            {
                _inserirVencimentoFlag = value;
                OnPropertyChanged("InserirVencimentoFlag");
            }
        }

        public ObservableCollection<MembroFamiliar> MembrosFamiliar
        {
            get
            {
                return _membrosFamiliar;
            }

            set
            {
                _membrosFamiliar = value;
                OnPropertyChanged("MembrosFamiliar");
            }
        }

        public bool IsCmbLojasEnabled
        {
            get
            {
                return _isCmbLojasEnabled;
            }

            set
            {
                _isCmbLojasEnabled = value;
                OnPropertyChanged("IsCmbLojasEnabled");
            }
        }
        public bool RegistrarCompraDeFornecedor
        {
            get
            {
                return _registrarCompraDeFornecedor;
            }

            set
            {
                _registrarCompraDeFornecedor = value;
                OnPropertyChanged("RegistrarCompraDeFornecedor");
            }
        }

        public bool PodeRegistrarCompraDeFornecedor
        {
            get
            {
                return _podeRegistrarCompraDeFornecedor;
            }

            set
            {
                _podeRegistrarCompraDeFornecedor = value;
                OnPropertyChanged("PodeRegistrarCompraDeFornecedor");
            }
        }
    }
}

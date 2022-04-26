using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

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
        protected DateTime? ultimaDataVencimento = null;

        private ObservableCollection<Model.TipoDespesa> _tiposDespesa;
        private ObservableCollection<Model.Fornecedor> _fornecedores;
        private ObservableCollection<Model.Representante> _representantes;
        private ObservableCollection<Model.Loja> _lojas;
        private ObservableCollection<Model.MembroFamiliar> _membrosFamiliar;

        public CadastrarDespesaVM(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            daoEntidade = new DAO<Model.Despesa>(session);
            daoTipoDespesa = new DAOTipoDespesa(session);
            daoFornecedor = new DAOFornecedor(session);
            daoRepresentante = new DAORepresentante(session);
            daoLoja = new DAOLoja(session);
            daoMembroFamiliar = new DAO<MembroFamiliar>(session);

            Entidade = new Model.Despesa()
            {
                Data = DateTime.Now
            };
            viewModelStrategy = new CadastrarDespesaVMStrategy();

            GetFornecedores();
            GetTiposDespesa();
            GetRepresentantes();
            GetLojas();
            GetMembrosFamiliar();
            Entidade.Descricao = TipoDescricao = "CONTA DE LUZ"; //Primeiro item

            PropertyChanged += CadastrarDespesaVM_PropertyChanged;
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

                        Entidade.Fornecedor = null;
                        Entidade.Familiar = MembrosFamiliar[0];
                        OnPropertyChanged("TipoDescricao");
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
                    if (Entidade.Representante != null && Entidade.Representante.Id == Guid.Empty)
                        Entidade.Representante = null;
                    break;
            }
        }

        public override void ResetaPropriedades(AposInserirBDEventArgs e)
        {
            Entidade = new Model.Despesa();
            Entidade.TipoDespesa = TiposDespesa[0];
            Entidade.Fornecedor = Fornecedores[0];
            Entidade.Loja = Lojas[0];
            TipoDescricao = "CONTA DE LUZ"; //Primeiro item
            Entidade.Data = DateTime.Now;
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
    }
}

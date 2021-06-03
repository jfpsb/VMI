using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Despesa
{
    public class CadastrarDespesaVM : ACadastrarViewModel<Model.Despesa>
    {
        private bool _isOutrosDespesas;
        private string _tipoDescricao;
        private DAOTipoDespesa daoTipoDespesa;
        private DAOFornecedor daoFornecedor;
        private DAORepresentante daoRepresentante;
        private Visibility _visibilidadeFornecedor;
        private Visibility _visibilidadeMembroFamiliar;

        private ObservableCollection<Model.TipoDespesa> _tiposDespesa;
        private ObservableCollection<Model.Fornecedor> _fornecedores;
        private ObservableCollection<Model.Representante> _representantes;

        public CadastrarDespesaVM(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            daoEntidade = new DAO<Model.Despesa>(session);
            daoTipoDespesa = new DAOTipoDespesa(session);
            daoFornecedor = new DAOFornecedor(session);
            daoRepresentante = new DAORepresentante(session);

            Entidade = new Model.Despesa()
            {
                Data = DateTime.Now,
                Familiar = "Ferreira"
            };
            viewModelStrategy = new CadastrarDespesaVMStrategy();

            GetFornecedores();
            GetTiposDespesa();
            GetRepresentantes();
            Entidade.Descricao = TipoDescricao = "CONTA DE LUZ"; //Primeiro item

            PropertyChanged += CadastrarDespesaVM_PropertyChanged;
        }

        private void CadastrarDespesaVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TipoDescricao":
                    if (TipoDescricao.Equals("OUTROS"))
                    {
                        IsOutrosDespesas = true;
                        Entidade.Descricao = string.Empty;
                    }
                    else
                    {
                        IsOutrosDespesas = false;
                        Entidade.Descricao = TipoDescricao;
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
                        VisibilidadeFornecedor = Visibility.Visible;
                        VisibilidadeMembroFamiliar = Visibility.Collapsed;

                        Entidade.Familiar = string.Empty;
                        Entidade.Fornecedor = Fornecedores[0];
                    }
                    else if (Entidade.TipoDespesa.Nome.Equals("DESPESA FAMILIAR"))
                    {
                        VisibilidadeFornecedor = Visibility.Collapsed;
                        VisibilidadeMembroFamiliar = Visibility.Visible;

                        Entidade.Fornecedor = null;
                        OnPropertyChanged("TipoDescricao");
                    }
                    else
                    {
                        VisibilidadeFornecedor = Visibility.Collapsed;
                        VisibilidadeMembroFamiliar = Visibility.Collapsed;

                        Entidade.Familiar = string.Empty;
                        Entidade.Fornecedor = null;
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

                case "Representante":
                    if (Entidade.Representante != null && Entidade.Representante.Id == 0)
                        Entidade.Representante = null;
                    break;
            }
        }

        public override void ResetaPropriedades()
        {
            Entidade = new Model.Despesa();
            Entidade.TipoDespesa = TiposDespesa[0];
            Entidade.Fornecedor = Fornecedores[0];
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

        public Visibility VisibilidadeFornecedor
        {
            get => _visibilidadeFornecedor;
            set
            {
                _visibilidadeFornecedor = value;
                OnPropertyChanged("VisibilidadeFornecedor");
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

        public ObservableCollection<Model.Representante> Representantes { get => _representantes; set => _representantes = value; }
    }
}

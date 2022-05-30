using NHibernate;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class CadastrarFornecedorManualmenteVM : ACadastrarViewModel<FornecedorModel>
    {
        private DAORepresentante daoRepresentante;
        private ObservableCollection<Model.Representante> _representantes;
        private Visibility _visibilidadeBotaoPesquisar;
        private Visibility _visibilidadeBotaoAtualizarReceita;

        public CadastrarFornecedorManualmenteVM(ISession session, bool issoEUmUpdate) : base(session, issoEUmUpdate)
        {
            _session = session;
            viewModelStrategy = new CadastrarFornecedorVMStrategy();
            daoEntidade = new DAOFornecedor(session);
            daoRepresentante = new DAORepresentante(session);
            Entidade = new FornecedorModel();
            Entidade.Cnpj = "0";
            IsEnabled = true;

            VisibilidadeBotaoPesquisar = Visibility.Collapsed;
            VisibilidadeBotaoAtualizarReceita = Visibility.Collapsed;

            GetRepresentantes();
        }

        public async override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cnpj":
                    var result = await daoEntidade.ListarPorId(Entidade.Cnpj);

                    if (result != null)
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Visible;
                        IsEnabled = false;
                    }
                    else
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Collapsed;
                        IsEnabled = true;
                    }

                    break;
                case "Representante":
                    if (Entidade.Representante != null && Entidade.Representante.Id == 0)
                        Entidade.Representante = null;
                    break;
            }
        }
        public override void ResetaPropriedades(AposInserirBDEventArgs e)
        {
            Entidade = new FornecedorModel();
            Entidade.Cnpj = Entidade.Nome = Entidade.Fantasia = Entidade.Email = string.Empty;
            Entidade.Representante = Representantes[0];
        }
        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (string.IsNullOrEmpty(Entidade.Nome))
            {
                BtnSalvarToolTip += "O Campo de Nome Não Pode Ser Vazio!\n";
                valido = false;
            }

            if (string.IsNullOrEmpty(Entidade.Cnpj))
            {
                BtnSalvarToolTip += "O Campo de CNPJ Não Pode Ser Vazio!\n";
                valido = false;
            }

            if (Entidade.Cnpj?.Length != 14)
            {
                BtnSalvarToolTip += "O Campo de CNPJ Deve Possuir 14 Dígitos!";
                valido = false;
            }

            return valido;
        }
        private async void GetRepresentantes()
        {
            Representantes = new ObservableCollection<Model.Representante>(await daoRepresentante.Listar());
            Representantes.Insert(0, new Model.Representante("SELECIONE UM REPRESENTANTE"));
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
        public Visibility VisibilidadeBotaoPesquisar
        {
            get => _visibilidadeBotaoPesquisar;
            set
            {
                _visibilidadeBotaoPesquisar = value;
                OnPropertyChanged("VisibilidadeBotaoPesquisar");
            }
        }
        public Visibility VisibilidadeBotaoAtualizarReceita
        {
            get => _visibilidadeBotaoAtualizarReceita;
            set
            {
                _visibilidadeBotaoAtualizarReceita = value;
                OnPropertyChanged("VisibilidadeBotaoAtualizarReceita");
            }
        }
    }
}

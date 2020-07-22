using NHibernate;
using System.ComponentModel;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.ViewModel.Marca
{
    class CadastrarMarcaViewModel : ACadastrarViewModel
    {
        protected DAOMarca daoMarca;
        private MarcaModel marcaModel;
        public CadastrarMarcaViewModel(ISession session)
        {
            _session = session;
            daoMarca = new DAOMarca(_session);
            marcaModel = new MarcaModel();
        }
        public MarcaModel Marca
        {
            get { return marcaModel; }
            set
            {
                marcaModel = value;
                OnPropertyChanged("Marca");
            }
        }

        public override async void Salvar(object parameter)
        {
            _result = await daoMarca.Inserir(marcaModel);

            if (_result)
            {
                ResetaPropriedades();
                await SetStatusBarSucesso("Marca Cadastrada Com Sucesso");
                return;
            }

            SetStatusBarErro("Erro ao Cadastrar Marca");
        }

        public override async void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Nome":
                    MarcaModel marca = (MarcaModel) await daoMarca.ListarPorId(Marca.Nome);

                    if (marca != null)
                    {
                        VisibilidadeAvisoItemJaExiste = System.Windows.Visibility.Visible;
                        IsEnabled = false;
                    }
                    else
                    {
                        VisibilidadeAvisoItemJaExiste = System.Windows.Visibility.Collapsed;
                        IsEnabled = true;
                    }

                    break;
            }
        }

        public override void ResetaPropriedades()
        {
            Marca = new MarcaModel();
            Marca.Nome = string.Empty;
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Marca.Nome?.Trim()))
                return false;

            return true;
        }
    }
}

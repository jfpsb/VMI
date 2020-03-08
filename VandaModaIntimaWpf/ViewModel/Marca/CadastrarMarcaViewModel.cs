using System.ComponentModel;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.ViewModel.Marca
{
    class CadastrarMarcaViewModel : ACadastrarViewModel
    {
        protected DAOMarca daoMarca;
        private MarcaModel marcaModel;
        public CadastrarMarcaViewModel() : base("Marca")
        {
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
            var result = await daoMarca.Inserir(marcaModel);

            if (result)
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

        public override bool ValidaModel(object parameter)
        {
            if (string.IsNullOrEmpty(Marca.Nome))
                return false;

            return true;
        }
    }
}

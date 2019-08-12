using System.ComponentModel;
using MarcaModel = VandaModaIntimaWpf.Model.Marca.Marca;

namespace VandaModaIntimaWpf.ViewModel.Marca
{
    class CadastrarMarcaViewModel : ACadastrarViewModel
    {
        private MarcaModel marcaModel;

        public MarcaModel Marca
        {
            get { return marcaModel; }
            set
            {
                marcaModel = value;
                OnPropertyChanged("Marca");
            }
        }

        public CadastrarMarcaViewModel()
        {
            marcaModel = new MarcaModel();
        }

        public override void Cadastrar(object parameter)
        {
            var result = marcaModel.Salvar();

            if (result)
            {
                MensagemStatusBar = "Marca Cadastrada Com Sucesso.";
                ImagemStatusBar = IMAGEMSUCESSO;
                ResetaPropriedades();
                return;
            }

            MensagemStatusBar = "Erro ao Cadastrar";
            ImagemStatusBar = IMAGEMERRO;
        }

        public override void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Nome":
                    MarcaModel marca = marcaModel.ListarPorId(Marca.Nome);

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

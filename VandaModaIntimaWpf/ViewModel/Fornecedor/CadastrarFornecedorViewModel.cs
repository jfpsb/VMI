using System.ComponentModel;
using System.Windows;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class CadastrarFornecedorViewModel : ACadastrarViewModel
    {
        private FornecedorModel fornecedor;
        public CadastrarFornecedorViewModel() : base()
        {
            fornecedor = new FornecedorModel();

            fornecedor.PropertyChanged += CadastrarViewModel_PropertyChanged;
        }
        public override void Cadastrar(object parameter)
        {
            var result = fornecedor.Salvar();

            if (result)
            {
                MensagemStatusBar = "Cadastro Realizado Com Sucesso";
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
                case "Cnpj":
                    var result = fornecedor.ListarPorId(Fornecedor.Cnpj);

                    if (result != null)
                    {
                        VisibilidadeAvisoCodBarra = Visibility.Visible;
                        IsEnabled = false;
                    }
                    else
                    {
                        VisibilidadeAvisoCodBarra = Visibility.Collapsed;
                        IsEnabled = true;
                    }

                    break;
            }
        }

        public override void ResetaPropriedades()
        {
            Fornecedor = new FornecedorModel();
            Fornecedor.Cnpj = Fornecedor.Nome = Fornecedor.NomeFantasia = Fornecedor.Email = string.Empty;
        }

        public override bool ValidaModel(object parameter)
        {
            if (string.IsNullOrEmpty(Fornecedor.Cnpj)
                || string.IsNullOrEmpty(Fornecedor.Nome))
            {
                return false;
            }

            return true;
        }

        public FornecedorModel Fornecedor
        {
            get { return fornecedor; }
            set
            {
                fornecedor = value;
                OnPropertyChanged("Fornecedor");
            }
        }
    }
}

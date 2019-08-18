using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class CadastrarFornecedorViewModel : ACadastrarViewModel
    {
        protected DAOFornecedor daoFornecedor;
        private FornecedorModel fornecedor;
        public CadastrarFornecedorViewModel() : base()
        {
            daoFornecedor = new DAOFornecedor(_session);
            fornecedor = new FornecedorModel();
            fornecedor.PropertyChanged += CadastrarViewModel_PropertyChanged;
        }
        public override async void Cadastrar(object parameter)
        {
            var result = await daoFornecedor.Inserir(fornecedor);

            if (result)
            {
                ResetaPropriedades();
                await SetStatusBarSucesso();
                return;
            }

            SetStatusBarErro();
        }

        public override void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cnpj":
                    var result = daoFornecedor.ListarPorId(Fornecedor.Cnpj);

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

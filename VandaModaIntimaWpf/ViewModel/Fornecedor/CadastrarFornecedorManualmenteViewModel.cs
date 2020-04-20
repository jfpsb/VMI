using NHibernate;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class CadastrarFornecedorManualmenteViewModel : ACadastrarViewModel
    {
        protected DAOFornecedor daoFornecedor;
        protected FornecedorModel fornecedor;
        public CadastrarFornecedorManualmenteViewModel(ISession session)
        {
            _session = session;
            daoFornecedor = new DAOFornecedor(_session);
            fornecedor = new FornecedorModel();
            fornecedor.PropertyChanged += CadastrarViewModel_PropertyChanged;
        }
        public override async void Salvar(object parameter)
        {
            _result = await daoFornecedor.Inserir(Fornecedor);

            if (_result)
            {
                ResetaPropriedades();
                await SetStatusBarSucesso("Fornecedor Cadastrado Com Sucesso");
                return;
            }

            SetStatusBarErro("Erro Ao Cadastrar Fornecedor");
        }

        public override async void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cnpj":
                    var result = await daoFornecedor.ListarPorId(Fornecedor.Cnpj);

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
            Fornecedor.Cnpj = Fornecedor.Nome = Fornecedor.Fantasia = Fornecedor.Email = string.Empty;
        }
        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Fornecedor.Cnpj)
                || string.IsNullOrEmpty(Fornecedor.Nome)
                || Fornecedor.Cnpj.Length != 14)
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

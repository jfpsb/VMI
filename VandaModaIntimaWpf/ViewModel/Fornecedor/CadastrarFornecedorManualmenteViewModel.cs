using NHibernate;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class CadastrarFornecedorManualmenteViewModel : ACadastrarViewModel<FornecedorModel>
    {
        protected DAOFornecedor daoFornecedor;

        public CadastrarFornecedorManualmenteViewModel(ISession session) : base(session)
        {
            cadastrarViewModelStrategy = new CadastrarFornecedorViewModelStrategy();
            daoFornecedor = new DAOFornecedor(_session);
            Entidade = new FornecedorModel();
            Entidade.PropertyChanged += CadastrarViewModel_PropertyChanged;
        }
        public override async void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cnpj":
                    var result = await daoFornecedor.ListarPorId(Entidade.Cnpj);

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
            Entidade = new FornecedorModel();
            Entidade.Cnpj = Entidade.Nome = Entidade.Fantasia = Entidade.Email = string.Empty;
        }
        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Entidade.Cnpj)
                || string.IsNullOrEmpty(Entidade.Nome)
                || Entidade.Cnpj.Length != 14)
            {
                return false;
            }

            return true;
        }

        protected override void ExecutarAntesCriarDocumento()
        {

        }
    }
}

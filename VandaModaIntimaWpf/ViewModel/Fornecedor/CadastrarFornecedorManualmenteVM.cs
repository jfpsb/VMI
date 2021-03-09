using NHibernate;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class CadastrarFornecedorManualmenteVM : ACadastrarViewModel<FornecedorModel>
    {
        public CadastrarFornecedorManualmenteVM(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            viewModelStrategy = new CadastrarFornecedorVMStrategy();
            daoEntidade = new DAOFornecedor(_session);
            Entidade = new FornecedorModel();
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
            }
        }

        public override void ResetaPropriedades()
        {
            Entidade = new FornecedorModel();
            Entidade.Cnpj = Entidade.Nome = Entidade.Fantasia = Entidade.Email = string.Empty;
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
    }
}

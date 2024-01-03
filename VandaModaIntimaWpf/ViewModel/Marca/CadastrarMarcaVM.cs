using NHibernate;
using System.ComponentModel;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.ViewModel.Marca
{
    class CadastrarMarcaVM : ACadastrarViewModel<MarcaModel>
    {
        public CadastrarMarcaVM(ISession session, bool isUpdate = false) : base(session, isUpdate)
        {
            viewModelStrategy = new CadastrarMarcaVMStrategy();
            daoEntidade = new DAOMarca(_session);
            Entidade = new MarcaModel();
        }

        public override void ResetaPropriedades(AposCRUDEventArgs e)
        {
            Entidade = new MarcaModel();
            Entidade.Nome = string.Empty;
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (string.IsNullOrEmpty(Entidade.Nome?.Trim()))
            {
                BtnSalvarToolTip += "Nome de Marca Não Pode Ser Vazio!";
                valido = false;
            }

            return valido;
        }

        public async override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Nome":
                    MarcaModel marca = (MarcaModel)await daoEntidade.ListarPorId(Entidade.Nome);

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
    }
}

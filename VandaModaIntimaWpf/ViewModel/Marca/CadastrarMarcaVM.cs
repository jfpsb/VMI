using NHibernate;
using System.ComponentModel;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.ViewModel.Marca
{
    class CadastrarMarcaVM : ACadastrarViewModel<MarcaModel>
    {
        public CadastrarMarcaVM(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            viewModelStrategy = new CadastrarMarcaVMStrategy();
            daoEntidade = new DAOMarca(_session);
            Entidade = new MarcaModel();
        }

        public override void ResetaPropriedades()
        {
            Entidade = new MarcaModel();
            Entidade.Nome = string.Empty;
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Entidade.Nome?.Trim()))
                return false;

            return true;
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

using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Representante
{
    public class PesquisarRepresentanteVM : APesquisarViewModel<Model.Representante>
    {
        public PesquisarRepresentanteVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<Model.Representante> abrePelaTelaPesquisaService) : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            daoEntidade = new DAORepresentante(_session);
            pesquisarViewModelStrategy = new PesquisarRepresentanteVMStrategy();
        }

        public override bool Editavel(object parameter)
        {
            return true;
        }

        public async override Task PesquisaItens(string termo)
        {
            if (termo != null)
            {
                DAORepresentante dao = (DAORepresentante)daoEntidade;
                Entidades = new System.Collections.ObjectModel.ObservableCollection<EntidadeComCampo<Model.Representante>>(EntidadeComCampo<Model.Representante>.CriarListaEntidadeComCampo(await dao.ListarPorNome(termo))); ;
            }
        }
    }
}

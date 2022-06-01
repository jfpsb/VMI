using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;

namespace VandaModaIntimaWpf.ViewModel.Representante
{
    public class PesquisarRepresentanteVM : APesquisarViewModel<Model.Representante>
    {
        public PesquisarRepresentanteVM()
        {
            daoEntidade = new DAORepresentante(_session);
            pesquisarViewModelStrategy = new PesquisarRepresentanteVMStrategy();
        }

        public override bool Editavel(object parameter)
        {
            return true;
        }

        public override object GetCadastrarViewModel()
        {
            return new CadastrarRepresentanteVM(_session);
        }

        public override object GetEditarViewModel()
        {
            return new EditarRepresentanteVM(_session, EntidadeSelecionada.Entidade);
        }

        public async override Task PesquisaItens(string termo)
        {
            if (termo != null)
            {
                DAORepresentante dao = (DAORepresentante)daoEntidade;
                Entidades = new System.Collections.ObjectModel.ObservableCollection<EntidadeComCampo<Model.Representante>>(EntidadeComCampo<Model.Representante>.CriarListaEntidadeComCampo(await dao.ListarPorNome(termo))); ;
            }
        }

        protected override WorksheetContainer<Model.Representante>[] GetWorksheetContainers()
        {
            throw new System.NotImplementedException();
        }
    }
}

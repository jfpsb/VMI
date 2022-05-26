using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using VandaModaIntimaWpf.ViewModel.SQL;

namespace VandaModaIntimaWpf.ViewModel.Representante
{
    public class PesquisarRepresentanteVM : APesquisarViewModel<Model.Representante>
    {
        public PesquisarRepresentanteVM(IMessageBoxService messageBoxService) : base(messageBoxService)
        {
            daoEntidade = new DAORepresentante(_session);
            pesquisarViewModelStrategy = new PesquisarRepresentanteVMStrategy();
        }

        public override bool Editavel(object parameter)
        {
            return true;
        }

        public override AAjudarVM GetAjudaVM()
        {
            throw new System.NotImplementedException();
        }

        public override ACadastrarViewModel<Model.Representante> GetCadastrarViewModel()
        {
            return new CadastrarRepresentanteVM(_session, MessageBoxService, false);
        }

        public override ACadastrarViewModel<Model.Representante> GetEditarViewModel()
        {
            return new EditarRepresentanteVM(_session, EntidadeSelecionada.Entidade, MessageBoxService);
        }

        public override ExportarSQLViewModel<Model.Representante> GetExportaSQLVM()
        {
            throw new System.NotImplementedException();
        }

        public override ATelaRelatorio GetTelaRelatorioVM()
        {
            throw new System.NotImplementedException();
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

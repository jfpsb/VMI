using System;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria
{
    class PesquisarEntradaVM : APesquisarViewModel<Model.EntradaDeMercadoria>
    {
        public PesquisarEntradaVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<Model.EntradaDeMercadoria> abrePelaTelaPesquisaService) : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            daoEntidade = new DAOEntradaDeMercadoria(_session);
            pesquisarViewModelStrategy = new PesquisarEntradaVMStrategy();
        }

        public override bool Editavel(object parameter)
        {
            return true;
        }

        public override Task PesquisaItens(string termo)
        {
            throw new NotImplementedException();
        }

        protected override WorksheetContainer<Model.EntradaDeMercadoria>[] GetWorksheetContainers()
        {
            return null;
        }
    }
}

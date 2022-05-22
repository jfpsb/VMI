using NHibernate;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.CompraDeFornecedor
{
    public class EditarCompraDeFornecedorVM : CadastrarCompraDeFornecedorVM
    {
        public EditarCompraDeFornecedorVM(ISession session, Model.CompraDeFornecedor compraDeFornecedor, IMessageBoxService messageBoxService) : base(session, messageBoxService, true)
        {
            _session = session;
            Entidade = compraDeFornecedor;
            daoEntidade = new DAO<Model.CompraDeFornecedor>(session);
            MessageBoxService = messageBoxService;
            viewModelStrategy = new EditarCompraDeFornecedorVMStrategy();
            Arquivos = new System.Collections.ObjectModel.ObservableCollection<Model.ArquivosCompraFornecedor>(Entidade.Arquivos);
        }
    }
}

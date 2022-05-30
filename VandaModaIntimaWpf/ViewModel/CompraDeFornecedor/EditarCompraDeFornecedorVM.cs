using NHibernate;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.ViewModel.CompraDeFornecedor
{
    public class EditarCompraDeFornecedorVM : CadastrarCompraDeFornecedorVM
    {
        public EditarCompraDeFornecedorVM(ISession session, Model.CompraDeFornecedor compraDeFornecedor) : base(session, true)
        {
            _session = session;
            Entidade = compraDeFornecedor;
            daoEntidade = new DAO<Model.CompraDeFornecedor>(session);
            viewModelStrategy = new EditarCompraDeFornecedorVMStrategy();
            Arquivos = new System.Collections.ObjectModel.ObservableCollection<Model.ArquivosCompraFornecedor>(Entidade.Arquivos);
        }
    }
}

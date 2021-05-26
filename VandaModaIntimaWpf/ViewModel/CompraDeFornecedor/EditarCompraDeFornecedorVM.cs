using NHibernate;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.CompraDeFornecedor
{
    public class EditarCompraDeFornecedorVM : CadastrarCompraDeFornecedorVM
    {
        public EditarCompraDeFornecedorVM(ISession session, Model.CompraDeFornecedor compraDeFornecedor, IMessageBoxService messageBoxService, IFileDialogService fileDialogService) : base(session, messageBoxService, fileDialogService, true)
        {
            _session = session;
            Entidade = compraDeFornecedor;
            daoEntidade = new DAO<Model.CompraDeFornecedor>(session);
            MessageBoxService = messageBoxService;
            _fileDialogService = fileDialogService;

            Arquivos = new System.Collections.ObjectModel.ObservableCollection<Model.ArquivosCompraFornecedor>(Entidade.Arquivos);
        }
    }
}

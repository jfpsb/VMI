using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel
{
    public interface IPesquisarMsgVMStrategy<E> where E : class, IModel
    {
        string MensagemApagarEntidadeCerteza(E e);
        string MensagemEntidadeDeletada(E e);
        string MensagemEntidadesDeletadas();
        string MensagemEntidadeNaoDeletada();
        string MensagemEntidadesNaoDeletadas();
        string MensagemApagarMarcados();
        string TelaApagarCaption();
        string MensagemDocumentoDeletado();
        string MensagemDocumentoNaoDeletado();
        string PesquisarEntidadeCaption();
    }
}

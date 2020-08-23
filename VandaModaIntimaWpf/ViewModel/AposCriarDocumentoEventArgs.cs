using VandaModaIntimaWpf.BancoDeDados;

namespace VandaModaIntimaWpf.ViewModel
{
    public class AposCriarDocumentoEventArgs : AposACadastrarViewModelEventArgs
    {
        public CouchDbResponse CouchDbResponse { get; set; }
    }
}

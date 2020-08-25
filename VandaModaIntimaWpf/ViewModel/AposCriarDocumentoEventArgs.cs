using VandaModaIntimaWpf.BancoDeDados;

namespace VandaModaIntimaWpf.ViewModel
{
    public class AposCriarDocumentoEventArgs : AposSalvarEditarEventArgs
    {
        public CouchDbResponse CouchDbResponse { get; set; }
    }
}

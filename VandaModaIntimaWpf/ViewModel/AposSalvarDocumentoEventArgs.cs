using VandaModaIntimaWpf.BancoDeDados;

namespace VandaModaIntimaWpf.ViewModel
{
    public class AposSalvarDocumentoEventArgs : AposSalvarEventArgs
    {
        
        public CouchDbResponse CouchDbResponse { get; set; }
    }
}

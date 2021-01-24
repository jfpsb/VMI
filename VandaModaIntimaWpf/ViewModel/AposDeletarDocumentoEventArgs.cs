using System;
using VandaModaIntimaWpf.BancoDeDados;

namespace VandaModaIntimaWpf.ViewModel
{
    public class AposDeletarDocumentoEventArgs : EventArgs
    {
        public CouchDbResponse CouchDbResponse { get; set; }
    }
}

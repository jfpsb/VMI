using System;
using VandaModaIntimaWpf.BancoDeDados;

namespace VandaModaIntimaWpf.ViewModel
{
    public class AposDeletarDocumentoEventArgs : EventArgs
    {
        public string MensagemSucesso { get; set; }
        public string MensagemErro { get; set; }
        public CouchDbResponse CouchDbResponse { get; set; }
    }
}

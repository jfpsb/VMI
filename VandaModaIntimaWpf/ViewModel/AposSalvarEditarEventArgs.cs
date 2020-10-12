using System;
using VandaModaIntimaWpf.BancoDeDados.Model;

namespace VandaModaIntimaWpf.ViewModel
{
    public class AposSalvarEditarEventArgs : EventArgs
    {
        public object IdentificadorEntidade { get; set; }
        public string MensagemSucesso { get; set; }
        public string MensagemErro { get; set; }
        public CouchDbLog CouchDbLog { get; set; }
    }
}

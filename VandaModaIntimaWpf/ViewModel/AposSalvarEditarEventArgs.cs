using System;
using VandaModaIntimaWpf.BancoDeDados.Model;

namespace VandaModaIntimaWpf.ViewModel
{
    public class AposSalvarEditarEventArgs : EventArgs
    {
        public object ObjetoSalvo { get; set; }
        public string MensagemSucesso { get; set; }
        public string MensagemErro { get; set; }
        public CouchDbLog CouchDbLog { get; set; }
    }
}

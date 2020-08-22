using System;

namespace VandaModaIntimaWpf.ViewModel
{
    public class AposCadastrarEventArgs : EventArgs
    {
        public bool SalvoComSucesso { get; set; }
        public object ObjetoSalvo { get; set; }
        public string MensagemSucesso { get; set; }
        public string MensagemErro { get; set; }
    }
}

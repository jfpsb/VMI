using System;

namespace VandaModaIntimaWpf.ViewModel
{
    public class AposACadastrarViewModelEventArgs : EventArgs
    {
        public object ObjetoSalvo { get; set; }
        public string MensagemSucesso { get; set; }
        public string MensagemErro { get; set; }
    }
}

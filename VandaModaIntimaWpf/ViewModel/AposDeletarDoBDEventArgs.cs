using System;

namespace VandaModaIntimaWpf.ViewModel
{
    public class AposDeletarDoBDEventArgs : EventArgs
    {
        public bool DeletadoComSucesso;
        public string MensagemSucesso { get; set; }
        public string MensagemErro { get; set; }
    }
}

using System;
using VandaModaIntimaWpf.BancoDeDados.Model;

namespace VandaModaIntimaWpf.ViewModel
{
    public class AposDeletarDoBDEventArgs : EventArgs
    {
        public bool DeletadoComSucesso;
        public string MensagemSucesso { get; set; }
        public string MensagemErro { get; set; }
    }
}

using System;

namespace VandaModaIntimaWpf.ViewModel
{
    public class AposSalvarEventArgs : EventArgs
    {
        public bool Sucesso { get; set; }
        public bool IsUpdate { get; set; }
    }
}

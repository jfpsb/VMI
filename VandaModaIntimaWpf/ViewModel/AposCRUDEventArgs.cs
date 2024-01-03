using System;

namespace VandaModaIntimaWpf.ViewModel
{
    public class AposCRUDEventArgs : EventArgs
    {
        public bool Sucesso;
        public bool IssoEhUpdate;
        public object UuidEntidade { get; set; }
        /// <summary>
        /// Parâmetro enviado junto com o comando da View para ViewModel
        /// </summary>
        public object Parametro { get; set; }
    }
}

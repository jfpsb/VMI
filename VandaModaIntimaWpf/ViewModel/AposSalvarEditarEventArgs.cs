using System;

namespace VandaModaIntimaWpf.ViewModel
{
    public class AposSalvarEditarEventArgs : EventArgs
    {
        public object IdentificadorEntidade { get; set; }
        public string MensagemSucesso { get; set; }
        public string MensagemErro { get; set; }

        /// <summary>
        /// Parâmetro enviado junto com o comando da View para ViewModel
        /// </summary>
        public object Parametro { get; set; }
    }
}

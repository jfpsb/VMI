using VandaModaIntimaWpf.BancoDeDados;

namespace VandaModaIntimaWpf.ViewModel
{
    public class AposInserirBDEventArgs : AposACadastrarViewModelEventArgs
    {
        public bool InseridoComSucesso;
        /// <summary>
        /// Indica se Operação foi um Update
        /// </summary>
        public bool IssoEUmUpdate;
        public CouchDbResponse CouchDbResponse;
    }
}

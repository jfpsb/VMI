namespace VandaModaIntimaWpf.ViewModel
{
    public class AposInserirBDEventArgs : AposSalvarEditarEventArgs
    {
        /// <summary>
        /// Indica se Operação foi um Update
        /// </summary>
        public bool IssoEUmUpdate;
        public bool Sucesso;
    }
}

using SincronizacaoBD.Model;

namespace SincronizacaoBD.Sincronizacao
{
    class DatabaseLogFileLoja : DatabaseLogFile
    {
        public Loja Entidade { get; set; }
    }
}

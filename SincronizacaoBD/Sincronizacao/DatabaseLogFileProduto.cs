using SincronizacaoBD.Model;

namespace SincronizacaoBD.Sincronizacao
{
    class DatabaseLogFileProduto : DatabaseLogFile
    {
        public Produto Entidade { get; set; }
    }
}

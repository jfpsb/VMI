using SincronizacaoBD.Model;

namespace SincronizacaoBD.Sincronizacao
{
    class DatabaseLogFileFornecedor : DatabaseLogFile
    {
        public Fornecedor Entidade { get; set; }
    }
}

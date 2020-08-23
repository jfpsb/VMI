namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbFornecedorLog : CouchDbLog
    {
        public string Cnpj { get; set; }
        public string Nome { get; set; }
        public string Fantasia { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public CouchDbFornecedorLog()
        {
            Tipo = "fornecedor";
        }
    }
}

using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbLojaLog : CouchDbLog
    {
        public string Cnpj { get; set; }
        public Loja Matriz { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string Inscricaoestadual { get; set; }
        public double Aluguel { get; set; }
        public CouchDbLojaLog()
        {
            Tipo = "loja";
        }
    }
}

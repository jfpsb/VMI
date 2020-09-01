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
        public override void AtribuiCampos(object o)
        {
            Loja l = (Loja)o;
            Cnpj = l.Cnpj;
            Matriz = l.Matriz;
            Nome = l.Nome;
            Telefone = l.Telefone;
            Endereco = l.Endereco;
            Inscricaoestadual = l.Endereco;
            Aluguel = l.Aluguel;
        }
        public override object Clone()
        {
            CouchDbLojaLog log = new CouchDbLojaLog()
            {
                Id = Id,
                Rev = Rev,
                Deleted = Deleted,
                Tipo = Tipo,
                RevsInfo = RevsInfo,
                Cnpj = Cnpj,
                Matriz = Matriz,
                Nome = Nome,
                Telefone = Telefone,
                Endereco = Endereco,
                Inscricaoestadual = Inscricaoestadual,
                Aluguel = Aluguel
            };

            return log;
        }
    }
}

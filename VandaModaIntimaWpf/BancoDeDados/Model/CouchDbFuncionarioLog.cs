using System.Collections.Generic;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbFuncionarioLog : CouchDbLog
    {
        public string Cpf { get; set; }
        public Loja Loja { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public double Salario { get; set; }
        public string Telefone { get; set; }
        public IList<FolhaPagamento> FolhaPagamentos { get; set; } = new List<FolhaPagamento>();
        public CouchDbFuncionarioLog()
        {
            Tipo = "funcionario";
        }
    }
}

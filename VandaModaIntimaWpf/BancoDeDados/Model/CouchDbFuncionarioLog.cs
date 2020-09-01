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
        public override void AtribuiCampos(object o)
        {
            Funcionario f = (Funcionario)o;
            Cpf = f.Cpf;
            Loja = f.Loja;
            Nome = f.Nome;
            Endereco = f.Endereco;
            Salario = f.Salario;
            Telefone = f.Telefone;
            FolhaPagamentos = new List<FolhaPagamento>(f.FolhaPagamentos);
        }
        public override object Clone()
        {
            CouchDbFuncionarioLog log = new CouchDbFuncionarioLog()
            {
                Id = Id,
                Rev = Rev,
                Deleted = Deleted,
                Tipo = Tipo,
                RevsInfo = RevsInfo,
                Cpf = Cpf,
                Loja = Loja,
                Nome = Nome,
                Endereco = Endereco,
                Salario = Salario,
                Telefone = Telefone,
                FolhaPagamentos = new List<FolhaPagamento>(FolhaPagamentos)
            };

            return log;
        }
    }
}

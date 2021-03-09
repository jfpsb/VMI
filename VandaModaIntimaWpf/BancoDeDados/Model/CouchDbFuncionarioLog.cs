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
            Telefone = f.Telefone;
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
            };

            return log;
        }
    }
}

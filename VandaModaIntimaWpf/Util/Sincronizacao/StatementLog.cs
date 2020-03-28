using System;
using System.Collections.Generic;
using System.Linq;

namespace VandaModaIntimaWpf.Util.Sincronizacao
{
    public class StatementLog
    {
        public string Tabela { get; set; }
        public string Statement { get; set; }
        public DateTime WriteTime { get; set; }
        public List<string> Identificadores { get; set; } = new List<string>();

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(StatementLog)) return false;

            StatementLog statement = (StatementLog)obj;

            bool b1 = statement.Identificadores.SequenceEqual(Identificadores);
            bool b2 = statement.Tabela.Equals(Tabela);

            return b1 && b2;
        }

        public override int GetHashCode()
        {
            return Identificadores.GetHashCode() + Tabela.GetHashCode();
        }
    }
}

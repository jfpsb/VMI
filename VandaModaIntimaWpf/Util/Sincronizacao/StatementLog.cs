using System;

namespace VandaModaIntimaWpf.Util.Sincronizacao
{
    public class StatementLog
    {
        public string Statement { get; set; }
        public DateTime WriteTime { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            StatementLog statement = (StatementLog)obj;

            if (statement.Statement.Equals(Statement))
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return Statement.GetHashCode();
        }
    }
}

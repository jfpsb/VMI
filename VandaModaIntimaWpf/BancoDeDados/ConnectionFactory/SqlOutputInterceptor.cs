using NHibernate;
using NHibernate.SqlCommand;
using System.Diagnostics;

namespace VandaModaIntimaWpf.BancoDeDados.ConnectionFactory
{
    public class SqlOutputInterceptor : EmptyInterceptor
    {
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            Debug.Write("NHibernate: ");
            Debug.WriteLine(sql);

            return base.OnPrepareStatement(sql);
        }
    }
}

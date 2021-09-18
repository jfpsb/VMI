using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOHistoricoProdutoGrade : DAO<HistoricoProdutoGrade>
    {
        public DAOHistoricoProdutoGrade(ISession session) : base(session)
        {
        }
    }
}

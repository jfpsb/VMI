using NHibernate;
using NHibernate.Type;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.BancoDeDados.ConnectionFactory
{
    public class CustomInterceptor : EmptyInterceptor
    {
        private IList<Tuple<string, byte[]>> _publishs;

        public CustomInterceptor()
        {
            _publishs = new List<Tuple<string, byte[]>>();
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            return base.OnSave(entity, id, state, propertyNames, types);
        }

        public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            base.OnDelete(entity, id, state, propertyNames, types);
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }

        public override void AfterTransactionCompletion(ITransaction tx)
        {
            if(tx != null && tx.WasCommitted)
            {

            }

            _publishs.Clear();

            base.AfterTransactionCompletion(tx);
        }
    }
}

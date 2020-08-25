using NHibernate;
using NHibernate.Type;

namespace VandaModaIntimaWpf.BancoDeDados.ConnectionFactory
{
    public class LocalInterceptor : EmptyInterceptor
    {
        //private static readonly string topico = "{0}/vandamodaintima/{1}/{2}/{3}";

        //private List<DatabaseLog> _logs;

        public LocalInterceptor()
        {
            //_logs = new List<DatabaseLog>();
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            //string t = string.Format(topico, GlobalConfigs.CLIENT_ID, entity.GetType().Name.ToLower(), entity.ToString(), "insert");

            //DatabaseLog log = new DatabaseLog()
            //{
            //    Topico = t,
            //    EnviadoAoServidor = false,
            //    Chaves = ((IModel)entity).DictionaryIdentifier
            //};

            //_logs.Add(log);

            return base.OnSave(entity, id, state, propertyNames, types);
        }

        public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            //string t = string.Format(topico, GlobalConfigs.CLIENT_ID, entity.GetType().Name.ToLower(), entity.ToString(), "delete");

            //DatabaseLog log = new DatabaseLog()
            //{
            //    Topico = t,
            //    EnviadoAoServidor = false,
            //    Chaves = ((IModel)entity).DictionaryIdentifier
            //};

            //_logs.Add(log);

            base.OnDelete(entity, id, state, propertyNames, types);
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            //string t = string.Format(topico, GlobalConfigs.CLIENT_ID, entity.GetType().Name.ToLower(), entity.ToString(), "update");

            //DatabaseLog log = new DatabaseLog()
            //{
            //    Topico = t,
            //    EnviadoAoServidor = false,
            //    Chaves = ((IModel)entity).DictionaryIdentifier
            //};

            //_logs.Add(log);

            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }

        public override void AfterTransactionCompletion(ITransaction tx)
        {
            //if (tx != null && MqttClientInit.MqttCliente != null && tx.WasCommitted)
            //{
            //    GlobalConfigs.DATABASELOG.AddRange(_logs);
            //    GlobalConfigs.SalvaDatabaseLog();
            //}

            //_logs.Clear();

            base.AfterTransactionCompletion(tx);
        }
    }
}

using NHibernate;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.BancoDeDados.ConnectionFactory
{
    public class SQLInterceptor : EmptyInterceptor
    {
        public override void AfterTransactionCompletion(ITransaction tx)
        {
            if (tx != null)
            {
                if (tx.WasCommitted)
                {
                    SincronizacaoViewModel.WriteStatementLog();
                    SincronizacaoViewModel.SendStatementLog();
                }
            }

            base.AfterTransactionCompletion(tx);
        }
    }
}

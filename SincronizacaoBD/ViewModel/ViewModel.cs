using SincronizacaoBD.Sincronizacao;
using System.Threading;

namespace SincronizacaoBD.ViewModel
{
    public class ViewModel : ObservableObject
    {
        private Thread threadBDRemoto;
        private string texto;

        public ViewModel()
        {
            SessionSyncProvider.MySessionFactory = SessionSyncProvider.BuildSessionFactoryLocal();
            SessionSyncProvider.MySessionFactorySync = SessionSyncProvider.BuildSessionFactorySync();

            threadBDRemoto = new Thread(() =>
            {
                Timer timer = null;

                timer = new Timer((e) =>
                {
                    SincronizacaoRemota.Sincronizar(AdicionaTexto);
                    timer.Change(5000, Timeout.Infinite);
                }, null, 0, Timeout.Infinite);
            });

            threadBDRemoto.Start();
        }

        public void AbortThread()
        {
            threadBDRemoto.Abort();
        }

        public void FechaSessionFactories()
        {
            SessionSyncProvider.FechaConexoesLocal();
            SessionSyncProvider.FechaConexoesSync();
        }

        public string Texto
        {
            get { return texto; }
            set
            {
                texto = value;
                OnPropertyChanged("Texto");
            }
        }

        private void AdicionaTexto(string texto)
        {
            Texto += texto;
        }
    }
}

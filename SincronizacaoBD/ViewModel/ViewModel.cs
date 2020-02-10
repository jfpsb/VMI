using SincronizacaoBD.Sincronizacao;
using System;
using System.Threading;

namespace SincronizacaoBD.ViewModel
{
    public class ViewModel : ObservableObject
    {
        private Thread threadBDRemoto;
        private string texto;

        public ViewModel()
        {
            threadBDRemoto = new Thread(() =>
            {
                Timer timerSessionFactory = null;
                Timer timerSincronizar = null;

                SessionSyncProvider.MySessionFactory = SessionSyncProvider.BuildSessionFactoryLocal();

                timerSessionFactory = new Timer((e) =>
                {
                    try
                    {
                        SessionSyncProvider.MySessionFactorySync = SessionSyncProvider.BuildSessionFactorySync();

                        timerSincronizar = new Timer((e2) =>
                        {
                            SincronizacaoRemota.Sincronizar(AdicionaTexto);
                            timerSincronizar.Change(0, Timeout.Infinite);
                        }, null, 0, Timeout.Infinite);
                    }
                    catch (Exception ex)
                    {
                        Texto += $"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}: Erro ao Conectar Com Servidor Remoto. Cheque Sua Conexão com a Internet:\n{ex.Message}\n";
                        timerSessionFactory.Change(30000, Timeout.Infinite);
                    }
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

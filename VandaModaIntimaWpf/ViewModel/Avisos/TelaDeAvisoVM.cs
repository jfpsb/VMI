using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.View.Ferias;
using VandaModaIntimaWpf.ViewModel.Interfaces;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Avisos
{
    public class TelaDeAvisoVM : ObservableObject, IViewModelClosing
    {
        private ObservableCollection<object> _itensAvisos;
        private DAOFerias daoFerias;
        private ISession session;
        private IMessageBoxService messageBoxService;

        public ICommand GerarComunicacaoComando { get; set; }

        public TelaDeAvisoVM()
        {
            session = SessionProvider.GetSession();
            daoFerias = new DAOFerias(session);
            messageBoxService = new MessageBoxService();
            ItensAvisos = new ObservableCollection<object>();

            //Consulta férias
            if (DateTime.Now.Day >= 20)
            {
                var ferias = daoFerias.RetornaFeriasParaComunicao().Result;

                foreach (var f in ferias)
                {
                    ItensAvisos.Add(f);
                }

                GerarComunicacaoComando = new RelayCommand(GerarComunicacao);
            }
        }

        private async void GerarComunicacao(object obj)
        {
            if (obj != null)
            {
                var ferias = obj as Model.Ferias;
                if (obj is Model.Ferias)
                {
                    TelaComunicacaoDeFerias telaComunicacaoDeFerias = new TelaComunicacaoDeFerias(ferias);
                    telaComunicacaoDeFerias.ShowDialog();

                    if (!ferias.Comunicado)
                    {
                        var dialogResult = messageBoxService.Show("Deseja marcar estas férias como comunicada definitivamente?", "Comunicação de férias", System.Windows.MessageBoxButton.YesNo,
                             System.Windows.MessageBoxImage.Question, System.Windows.MessageBoxResult.No);

                        if (dialogResult == System.Windows.MessageBoxResult.Yes)
                        {
                            ferias.Comunicado = true;
                            try
                            {
                                await daoFerias.Atualizar(obj);
                            }
                            catch (Exception ex)
                            {
                                messageBoxService.Show($"Erro ao salvar férias. Para mais detalhes acesse {Log.LogBanco}.\n\n{ex.Message}", "Comunicação de férias", System.Windows.MessageBoxButton.OK,
                                     System.Windows.MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        public ObservableCollection<object> ItensAvisos
        {
            get
            {
                return _itensAvisos;
            }

            set
            {
                _itensAvisos = value;
                OnPropertyChanged("ItensAvisos");
            }
        }

        public void OnClosing()
        {
            SessionProvider.FechaSession(session);
        }
    }
}

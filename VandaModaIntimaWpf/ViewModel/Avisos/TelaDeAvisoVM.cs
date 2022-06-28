using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.View.Ferias;
using VandaModaIntimaWpf.ViewModel.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Avisos
{
    public class TelaDeAvisoVM : ObservableObject, IViewModelClosing
    {
        private ObservableCollection<object> _itensAvisos;
        private DAOFerias daoFerias;
        private ISession session;

        public ICommand GerarComunicacaoComando { get; set; }

        public TelaDeAvisoVM()
        {
            session = SessionProvider.GetSession();
            daoFerias = new DAOFerias(session);
            ItensAvisos = new ObservableCollection<object>();

            //Consulta férias
            var ferias = daoFerias.RetornaFeriasParaComunicao().Result;

            foreach (var f in ferias)
            {
                ItensAvisos.Add(f);
            }

            GerarComunicacaoComando = new RelayCommand(GerarComunicacao);
        }

        private void GerarComunicacao(object obj)
        {
            if (obj != null)
            {
                if (obj is Model.Ferias)
                {
                    TelaComunicacaoDeFerias telaComunicacaoDeFerias = new TelaComunicacaoDeFerias(obj as Model.Ferias);
                    telaComunicacaoDeFerias.ShowDialog();
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

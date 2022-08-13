using NHibernate;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO.Pix;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.ViewModel.Pix
{
    public class PagamentoPixVM : ObservableObject
    {
        private ISession _session;
        private DAOPix daoPix;
        private ObservableCollection<Model.Pix.Pix> _listaPix;

        public PagamentoPixVM()
        {
            _session = SessionProvider.GetSession();
            daoPix = new DAOPix(_session);

            var task = GetListaPix();
            task.Wait();
        }

        private async Task GetListaPix()
        {
            ListaPix = new ObservableCollection<Model.Pix.Pix>(await daoPix.ListarPixPorDiaLoja(System.DateTime.Now, GetLojaAplicacao.LojaAplicacao(_session)));
        }

        public void FechaSession()
        {
            SessionProvider.FechaSession(_session);
        }

        public ObservableCollection<Model.Pix.Pix> ListaPix
        {
            get
            {
                return _listaPix;
            }

            set
            {
                _listaPix = value;
                OnPropertyChanged("ListaPix");
            }
        }
    }
}

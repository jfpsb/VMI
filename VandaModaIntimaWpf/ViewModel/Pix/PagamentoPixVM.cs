using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private double _valorQrCodePix;

        public ICommand GerarQRCodeComando { get; set; }

        public PagamentoPixVM()
        {
            _session = SessionProvider.GetSession();
            daoPix = new DAOPix(_session);

            var task = GetListaPix();
            task.Wait();

            GerarQRCodeComando = new RelayCommand(GerarQRCode, GerarQRCodeValidacao);
        }

        private void GerarQRCode(object obj)
        {
            throw new NotImplementedException();
        }

        private bool GerarQRCodeValidacao(object arg)
        {
            return ValorQrCodePix > 0;
        }

        private async Task GetListaPix()
        {
            var dt = new DateTime(2022, 8, 5);
            ListaPix = new ObservableCollection<Model.Pix.Pix>(await daoPix.ListarPixPorDiaLoja(dt, GetLojaAplicacao.LojaAplicacao(_session)));
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

        public double ValorQrCodePix
        {
            get
            {
                return _valorQrCodePix;
            }

            set
            {
                _valorQrCodePix = value;
                OnPropertyChanged("ValorQrCodePix");
            }
        }
    }
}

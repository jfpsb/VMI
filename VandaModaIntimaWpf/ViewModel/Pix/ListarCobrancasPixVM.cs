using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO.Pix;
using VandaModaIntimaWpf.Model.Pix;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Pix
{
    public class ListarCobrancasPixVM : ObservableObject
    {
        private ISession _session;
        private ObservableCollection<Cobranca> _cobrancas;
        private DAOCobranca daoCobranca;
        private IWindowService windowService;

        public ICommand ListViewCobrancaLeftMouseClickComando { get; set; }

        public ListarCobrancasPixVM(ISession session)
        {
            _session = session;
            daoCobranca = new DAOCobranca(_session);
            windowService = new WindowService();
            var task = GetCobrancas();
            task.Wait();

            ListViewCobrancaLeftMouseClickComando = new RelayCommand(ListViewLeftMouseClick);
        }

        private async Task GetCobrancas()
        {
            var lojaApp = Config.LojaAplicacao(_session);
            Cobrancas = new ObservableCollection<Cobranca>(await daoCobranca.ListarPorDiaELoja(DateTime.Now, lojaApp));
        }

        private void ListViewLeftMouseClick(object obj)
        {
            if (obj != null)
            {
                ApresentaQRCodePixVM viewModel = new ApresentaQRCodePixVM(_session, ((Cobranca)obj).Id, new MessageBoxService());
                windowService.ShowDialog(viewModel, null);
            }
        }

        public ObservableCollection<Cobranca> Cobrancas
        {
            get
            {
                return _cobrancas;
            }

            set
            {
                _cobrancas = value;
            }
        }
    }
}

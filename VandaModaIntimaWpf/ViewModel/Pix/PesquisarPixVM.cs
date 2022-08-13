using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;

namespace VandaModaIntimaWpf.ViewModel.Pix
{
    public class PesquisarPixVM : APesquisarViewModel<Model.Pix.Pix>
    {
        private DateTime _dataEscolhida;
        private Model.Loja _loja;
        private ObservableCollection<Model.Loja> _lojas;
        private ObservableCollection<Model.Pix.Pix> _listaPix;
        private DAOLoja daoLoja;
        private double _totalPorQRCode;
        private double _totalPorChave;
        public PesquisarPixVM()
        {
            daoEntidade = new DAOPix(_session);
            daoLoja = new DAOLoja(_session);
            pesquisarViewModelStrategy = new PesquisarPixVMStrategy();

            DataEscolhida = DateTime.Now.AddDays(-2);

            var task = GetLojas();
            task.Wait();

            PropertyChanged += PesquisarPixVM_PropertyChanged;

            OnPropertyChanged("TermoPesquisa");
        }

        private void PesquisarPixVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("DataEscolhida") || e.PropertyName.Equals("Loja"))
            {
                OnPropertyChanged("TermoPesquisa");
            }
        }

        private async Task GetLojas()
        {
            Lojas = new ObservableCollection<Model.Loja>(await daoLoja.ListarSomenteLojas());
            Loja = Lojas[0];
        }

        public override bool Editavel(object parameter)
        {
            return false;
        }

        public override object GetCadastrarViewModel()
        {
            throw new NotImplementedException();
        }

        public override object GetEditarViewModel()
        {
            throw new NotImplementedException();
        }

        public async override Task PesquisaItens(string termo)
        {
            ListaPix = new ObservableCollection<Model.Pix.Pix>(await (daoEntidade as DAOPix).ListarPixPorDiaLoja(DataEscolhida, Loja));
            TotalPorChave = ListaPix.Where(w => w.Cobranca == null).Sum(s => s.Valor);
            TotalPorQRCode = ListaPix.Where(w => w.Cobranca != null).Sum(s => s.Valor);
        }

        protected override WorksheetContainer<Model.Pix.Pix>[] GetWorksheetContainers()
        {
            throw new NotImplementedException();
        }

        public DateTime DataEscolhida
        {
            get
            {
                return _dataEscolhida;
            }

            set
            {
                _dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
            }
        }

        public Model.Loja Loja
        {
            get
            {
                return _loja;
            }

            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
            }
        }

        public ObservableCollection<Model.Loja> Lojas
        {
            get
            {
                return _lojas;
            }

            set
            {
                _lojas = value;
                OnPropertyChanged("Lojas");
            }
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

        public double TotalPorQRCode
        {
            get
            {
                return _totalPorQRCode;
            }

            set
            {
                _totalPorQRCode = value;
                OnPropertyChanged("TotalPorQRCode");
                OnPropertyChanged("TotalGeral");
            }
        }

        public double TotalPorChave
        {
            get
            {
                return _totalPorChave;
            }

            set
            {
                _totalPorChave = value;
                OnPropertyChanged("TotalPorChave");
                OnPropertyChanged("TotalGeral");
            }
        }

        public double TotalGeral
        {
            get => TotalPorQRCode + TotalPorChave;
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;

namespace VandaModaIntimaWpf.ViewModel.VendaEmCartao
{
    public class PesquisaParcelaCartaoVM : APesquisarViewModel<Model.ParcelaCartao>
    {
        private ObservableCollection<EntidadeComCampo<Model.Loja>> _lojasComCampo;
        private ObservableCollection<Model.ParcelaCartao> _parcelas;
        private DAOParcelaCartao daoParcelaCartao;
        private DAOLoja daoLoja;
        private DateTime _dataEscolhida;
        private double _totalBruto;
        private double _totalLiquido;
        private EntidadeComCampo<Model.Loja> _primeiroSelecionado;
        private bool _agruparPorDia;
        private bool _agruparPorOperadora;

        public PesquisaParcelaCartaoVM()
        {
            daoParcelaCartao = new DAOParcelaCartao(Session);
            daoLoja = new DAOLoja(Session);
            pesquisarViewModelStrategy = new PesquisarParcelaCartaoVMStrategy();
            Parcelas = new ObservableCollection<ParcelaCartao>();
            LojasComCampo = new ObservableCollection<EntidadeComCampo<Model.Loja>>();

            DataEscolhida = DateTime.Now;
            TotalBruto = 0;
            TotalLiquido = 0;

            var task1 = GetLojas();
            task1.Wait();

            PropertyChanged += PesquisaParcelaCartaoVM_PropertyChanged;
        }

        private void PesquisaParcelaCartaoVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "DataEscolhida":
                    OnPropertyChanged("TermoPesquisa");
                    break;
                case "LojasComCampo":
                    OnPropertyChanged("TermoPesquisa");
                    break;
                case "AgruparPorDia":
                    OnPropertyChanged("TermoPesquisa");
                    break;
                case "AgruparPorOperadora":
                    OnPropertyChanged("TermoPesquisa");
                    break;
            }
        }

        private async Task GetLojas()
        {
            foreach (var item in LojasComCampo)
            {
                item.PropertyChanged -= LojaComCampo_PropertyChanged;
            }

            LojasComCampo = new ObservableCollection<EntidadeComCampo<Model.Loja>>(EntidadeComCampo<Model.Loja>.CriarListaEntidadeComCampo(await daoLoja.ListarSomenteLojas()));

            foreach (var item in LojasComCampo)
            {
                item.PropertyChanged += LojaComCampo_PropertyChanged;
            }

            LojasComCampo[0].IsChecked = true; //Marca primeira loja
            PrimeiraLojaSelecionada = LojasComCampo[0];
        }

        private void LojaComCampo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsChecked"))
            {
                OnPropertyChanged("TermoPesquisa");
                var first = LojasComCampo.FirstOrDefault(f => f.IsChecked);
                if (first != null)
                    PrimeiraLojaSelecionada = first;
            }
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

        public override async Task PesquisaItens(string termo)
        {
            Parcelas.Clear();
            var lojas = LojasComCampo.Where(w => w.IsChecked).Select(s => s.Entidade).ToArray();

            if (lojas.Length == 0)
            {
                LojasComCampo[0].IsChecked = true;
                lojas = new Model.Loja[1];
                lojas[0] = LojasComCampo[0].Entidade;
            }

            if (AgruparPorDia || AgruparPorOperadora)
            {
                Parcelas = new ObservableCollection<ParcelaCartao>(await daoParcelaCartao.ListarPorMesLojasGroupByDiaOperadora(DataEscolhida, AgruparPorDia, AgruparPorOperadora, lojas));
            }
            else
            {
                Parcelas = new ObservableCollection<ParcelaCartao>(await daoParcelaCartao.ListarPorMesLojas(DataEscolhida, lojas));
            }

            TotalBruto = Parcelas.Sum(s => s.ValorBruto);
            TotalLiquido = Parcelas.Sum(s => s.ValorLiquido);
        }

        protected override WorksheetContainer<ParcelaCartao>[] GetWorksheetContainers()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<EntidadeComCampo<Model.Loja>> LojasComCampo
        {
            get
            {
                return _lojasComCampo;
            }

            set
            {
                _lojasComCampo = value;
                OnPropertyChanged("LojasComCampo");
            }
        }

        public ObservableCollection<ParcelaCartao> Parcelas
        {
            get
            {
                return _parcelas;
            }

            set
            {
                _parcelas = value;
                OnPropertyChanged("Parcelas");
            }
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

        public double TotalBruto
        {
            get
            {
                return _totalBruto;
            }

            set
            {
                _totalBruto = value;
                OnPropertyChanged("TotalBruto");
            }
        }

        public double TotalLiquido
        {
            get
            {
                return _totalLiquido;
            }

            set
            {
                _totalLiquido = value;
                OnPropertyChanged("TotalLiquido");
            }
        }

        public EntidadeComCampo<Model.Loja> PrimeiraLojaSelecionada
        {
            get
            {
                return _primeiroSelecionado;
            }

            set
            {
                _primeiroSelecionado = value;
                OnPropertyChanged("PrimeiraLojaSelecionada");
            }
        }

        public bool AgruparPorDia
        {
            get
            {
                return _agruparPorDia;
            }

            set
            {
                _agruparPorDia = value;
                OnPropertyChanged("AgruparPorDia");
            }
        }

        public bool AgruparPorOperadora
        {
            get
            {
                return _agruparPorOperadora;
            }

            set
            {
                _agruparPorOperadora = value;
                OnPropertyChanged("AgruparPorOperadora");
            }
        }
    }
}

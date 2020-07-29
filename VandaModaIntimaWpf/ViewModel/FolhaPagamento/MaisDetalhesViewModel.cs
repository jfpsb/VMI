using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.View;
using FolhaModel = VandaModaIntimaWpf.Model.FolhaPagamento;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    //TODO: Colocar StatusBar em app.xaml
    public class MaisDetalhesViewModel : ObservableObject, IResultReturnable
    {
        private ObservableCollection<Parcela> _parcelas;
        private ObservableCollection<Bonus> _bonus;
        private DAOAdiantamento daoAdiantamento;
        private DAOBonus daoBonus;
        private ISession _session;
        private Parcela _parcela;
        private Bonus _bonusEscolhido;
        private string _mensagemStatusBar;
        private BitmapImage _imagemStatusBar;
        private FolhaModel _folha;
        private bool? _dialogResult = false;

        public ICommand DeletarAdiantamentoComando { get; set; }
        public ICommand DeletarBonusComando { get; set; }
        public MaisDetalhesViewModel(ISession session, FolhaModel folhaPagamento)
        {
            _session = session;
            _folha = folhaPagamento;
            daoAdiantamento = new DAOAdiantamento(session);
            daoBonus = new DAOBonus(session);
            Parcelas = new ObservableCollection<Parcela>(folhaPagamento.Parcelas);
            Bonus = new ObservableCollection<Bonus>(folhaPagamento.Bonus);
            DeletarAdiantamentoComando = new RelayCommand(DeletarAdiantamento);
            DeletarBonusComando = new RelayCommand(DeletarBonus);
        }

        private async void DeletarBonus(object obj)
        {
            //TODO: Colocar abertura de view em classe separada
            //TODO: Colocar strings em resources
            TelaApagarDialog telaApagarDialog = new TelaApagarDialog(string.Format("Tem Certeza Que Deseja Apagar o Bônus Criado Em {0}?", BonusEscolhido.DataString),
                "Deletar Bônus");

            bool? _result = telaApagarDialog.ShowDialog();

            if (_result == true)
            {
                _folha.Bonus.Remove(BonusEscolhido);

                bool resultadoDelete = await daoBonus.Deletar(BonusEscolhido);

                if (resultadoDelete)
                {
                    SetStatusBarItemDeletado("Bônus Deletado Com Sucesso");
                }
            }
        }

        private async void DeletarAdiantamento(object obj)
        {
            //TODO: Colocar abertura de view em classe separada
            //TODO: Colocar strings em resources
            TelaApagarDialog telaApagarDialog = new TelaApagarDialog(string.Format("Tem Certeza Que Deseja Apagar o Adiantamento Criado Em {0}?\nTodas as Parcelas Serão Deletadas!", Parcela.Adiantamento.DataString),
                "Deletar Adiantamento");

            bool? _result = telaApagarDialog.ShowDialog();

            if (_result == true)
            {
                foreach (var p in Parcela.Adiantamento.Parcelas)
                {
                    FolhaModel f = p.FolhaPagamento;
                    f.Parcelas.Remove(p);
                }

                bool resultadoDelete = await daoAdiantamento.Deletar(Parcela.Adiantamento);

                if (resultadoDelete)
                {
                    SetStatusBarItemDeletado("Adiantamento Deletado Com Sucesso");
                }
            }
        }

        public async void SetStatusBarItemDeletado(string mensagem)
        {
            _dialogResult = true;
            MensagemStatusBar = mensagem;
            ImagemStatusBar = GetResource.GetBitmapImage("ImagemDeletado");
            await _session.RefreshAsync(_folha);
            Parcelas = new ObservableCollection<Parcela>(_folha.Parcelas);
            Bonus = new ObservableCollection<Bonus>(_folha.Bonus);
            await ResetarStatusBar();
        }
        public async Task ResetarStatusBar()
        {
            await Task.Delay(7000); //Espera 7 segundos para resetar StatusBar
            SetStatusBarAguardando();
        }
        public void SetStatusBarAguardando()
        {
            MensagemStatusBar = GetResource.GetString("aguardando_usuario");
            ImagemStatusBar = GetResource.GetBitmapImage("ImagemAguardando");
        }

        public bool? DialogResult()
        {
            return _dialogResult;
        }

        public BitmapImage ImagemStatusBar
        {
            get { return _imagemStatusBar; }
            set
            {
                _imagemStatusBar = value;
                OnPropertyChanged("ImagemStatusBar");
            }
        }

        public ObservableCollection<Parcela> Parcelas
        {
            get => _parcelas;
            set
            {
                _parcelas = value;
                OnPropertyChanged("Parcelas");
            }
        }

        public Parcela Parcela
        {
            get => _parcela;
            set
            {
                _parcela = value;
                OnPropertyChanged("Parcela");
            }
        }

        public string MensagemStatusBar
        {
            get => _mensagemStatusBar;
            set
            {
                _mensagemStatusBar = value;
                OnPropertyChanged("MensagemStatusBar");
            }
        }

        public ObservableCollection<Bonus> Bonus
        {
            get => _bonus;
            set
            {
                _bonus = value;
                OnPropertyChanged("Bonus");
            }
        }

        public Bonus BonusEscolhido
        {
            get => _bonusEscolhido;
            set
            {
                _bonusEscolhido = value;
                OnPropertyChanged("BonusEscolhido");
            }
        }
    }
}

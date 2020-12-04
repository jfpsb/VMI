using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FolhaModel = VandaModaIntimaWpf.Model.FolhaPagamento;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    //TODO: Colocar StatusBar em app.xaml
    public class MaisDetalhesVM : ObservableObject, IResultReturnable
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
        private IMessageBoxService MessageBoxService;

        public ICommand DeletarAdiantamentoComando { get; set; }
        public ICommand DeletarBonusComando { get; set; }
        public MaisDetalhesVM(ISession session, FolhaModel folhaPagamento, IMessageBoxService messageBoxService)
        {
            MessageBoxService = messageBoxService;

            _session = session;
            _folha = folhaPagamento;
            daoAdiantamento = new DAOAdiantamento(session);
            daoBonus = new DAOBonus(session);
            DeletarAdiantamentoComando = new RelayCommand(DeletarAdiantamento);
            DeletarBonusComando = new RelayCommand(DeletarBonus);
        }

        private async void DeletarBonus(object obj)
        {
            //TODO: Colocar strings em resources
            MessageBoxResult telaApagar = MessageBoxService.Show(string.Format("Tem Certeza Que Deseja Apagar o Bônus Criado Em {0}?", BonusEscolhido.DataString),
                "Deletar Bônus",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (telaApagar.Equals(MessageBoxResult.Yes))
            {
                bool resultadoDelete = await daoBonus.Deletar(BonusEscolhido);

                if (resultadoDelete)
                {
                    SetStatusBarItemDeletado("Bônus Deletado Com Sucesso");
                }
            }
        }

        private async void DeletarAdiantamento(object obj)
        {
            //TODO: Colocar strings em resources
            MessageBoxResult telaApagar = MessageBoxService.Show(string.Format("Tem Certeza Que Deseja Apagar o Adiantamento Criado Em {0}?\nTodas as Parcelas Serão Deletadas!", Parcela.Adiantamento.DataString),
                "Deletar Adiantamento",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (telaApagar.Equals(MessageBoxResult.Yes))
            {
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

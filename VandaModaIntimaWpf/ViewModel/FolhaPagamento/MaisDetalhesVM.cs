using NHibernate;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FolhaModel = VandaModaIntimaWpf.Model.FolhaPagamento;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class MaisDetalhesVM : ObservableObject, IResultReturnable
    {
        private ObservableCollection<Parcela> _parcelas;
        private ObservableCollection<Bonus> _bonus;
        private DAOAdiantamento daoAdiantamento;
        private DAOBonus daoBonus;
        private ISession _session;
        private Parcela _parcela;
        private Bonus _bonusEscolhido;
        private FolhaModel _folhaPagamento;
        private bool? _dialogResult = false;
        private IMessageBoxService MessageBoxService;

        public ICommand DeletarAdiantamentoComando { get; set; }
        public ICommand DeletarBonusComando { get; set; }
        public MaisDetalhesVM(ISession session, FolhaModel folhaPagamento, IMessageBoxService messageBoxService)
        {
            MessageBoxService = messageBoxService;

            _session = session;
            FolhaPagamento = folhaPagamento;
            daoAdiantamento = new DAOAdiantamento(session);
            daoBonus = new DAOBonus(session);
            DeletarAdiantamentoComando = new RelayCommand(DeletarAdiantamento);
            DeletarBonusComando = new RelayCommand(DeletarBonus);

            Parcelas = new ObservableCollection<Parcela>(FolhaPagamento.Parcelas);
            Bonus = new ObservableCollection<Bonus>(FolhaPagamento.Bonus);
        }

        private async void DeletarBonus(object obj)
        {
            //TODO: Colocar strings em resources
            MessageBoxResult telaApagar = MessageBoxService.Show(string.Format("Tem Certeza Que Deseja Apagar o Bônus Criado Em {0}?", BonusEscolhido.Data.ToString("dd/MM/yyyy")),
                "Deletar Bônus",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (telaApagar.Equals(MessageBoxResult.Yes))
            {
                bool result;
                if (BonusEscolhido.BonusMensal)
                {
                    BonusEscolhido.BonusCancelado = true;
                    result = await daoBonus.InserirOuAtualizar(BonusEscolhido);
                }
                else
                {
                    result = await daoBonus.Deletar(BonusEscolhido);
                }

                if (result)
                {
                    MessageBoxService.Show("Bônus Deletado Com Sucesso");
                    Bonus.Remove(BonusEscolhido);
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
                FolhaPagamento.Funcionario.Adiantamentos.Remove(Parcela.Adiantamento);

                bool resultadoDelete = await daoAdiantamento.Deletar(Parcela.Adiantamento);

                if (resultadoDelete)
                {
                    MessageBoxService.Show("Adiantamento Deletado Com Sucesso!");
                    _session.Refresh(FolhaPagamento);
                    Parcelas = new ObservableCollection<Parcela>(FolhaPagamento.Parcelas);
                }
            }
        }

        public bool? DialogResult()
        {
            return _dialogResult;
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

        public ObservableCollection<Bonus> Bonus
        {
            get => _bonus;
            set
            {
                _bonus = value;
                OnPropertyChanged("Bonus");
            }
        }

        public double TotalBonus
        {
            get => FolhaPagamento.TotalBonus;
        }

        public double TotalParcelas
        {
            get => FolhaPagamento.Parcelas.Sum(s => s.Valor);
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

        public FolhaModel FolhaPagamento
        {
            get => _folhaPagamento;
            set
            {
                _folhaPagamento = value;
                OnPropertyChanged("FolhaPagamento");
            }
        }
    }
}

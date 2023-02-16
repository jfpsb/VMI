using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FolhaModel = VandaModaIntimaWpf.Model.FolhaPagamento;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class MaisDetalhesVM : ObservableObject, IDialogResult
    {
        private ISession _session;
        private ObservableCollection<Parcela> _parcelas;
        private ObservableCollection<Bonus> _bonus;
        private DAOFolhaPagamento daoFolha;
        private DAOBonus daoBonus;
        private DAOParcela daoParcela;
        private DAO<Adiantamento> daoAdiantamento;
        private Parcela _parcela;
        private Bonus _bonusEscolhido;
        private FolhaModel _folhaPagamento;
        private bool? _dialogResult = false;
        private IMessageBoxService MessageBoxService;
        private IWindowService _windowService;

        public ICommand DeletarAdiantamentoComando { get; set; }
        public ICommand DeletarBonusComando { get; set; }
        public ICommand GerenciarParcelasComando { get; set; }
        public MaisDetalhesVM(ISession session, FolhaModel folhaPagamento)
        {
            _session = session;
            MessageBoxService = new MessageBoxService();
            _windowService = new WindowService();

            FolhaPagamento = folhaPagamento;
            daoBonus = new DAOBonus(session);
            daoFolha = new DAOFolhaPagamento(session);
            daoParcela = new DAOParcela(session);
            daoAdiantamento = new DAO<Adiantamento>(session);
            DeletarAdiantamentoComando = new RelayCommand(DeletarAdiantamento);
            DeletarBonusComando = new RelayCommand(DeletarBonus);
            GerenciarParcelasComando = new RelayCommand(GerenciarParcelas);

            Parcelas = new ObservableCollection<Parcela>(FolhaPagamento.Parcelas);
            Bonus = new ObservableCollection<Bonus>(FolhaPagamento.Bonus);
        }

        private void GerenciarParcelas(object obj)
        {
            _windowService.ShowDialog(new GerenciarParcelasVM(_session, Parcela.Adiantamento, MessageBoxService), async (result, viewModel) =>
            {
                _dialogResult = result;
                if (result == true)
                {
                    var parc = await daoParcela.ListarPorFuncionarioMesAno(FolhaPagamento.Funcionario, FolhaPagamento.Mes, FolhaPagamento.Ano);
                    Parcelas = new ObservableCollection<Parcela>(parc);
                }
                OnPropertyChanged("TotalParcelas");
            });
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
                try
                {
                    if (BonusEscolhido.BonusMensal)
                    {
                        BonusEscolhido.BonusCancelado = true;
                        await daoBonus.InserirOuAtualizar(BonusEscolhido);
                    }
                    else
                    {
                        await daoBonus.Deletar(BonusEscolhido);
                    }

                    Bonus.Remove(BonusEscolhido);
                    _dialogResult = true;
                    OnPropertyChanged("TotalBonus");
                    MessageBoxService.Show("Bônus Deletado Com Sucesso", "Mais Detalhes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBoxService.Show($"Erro ao deletar bônus. Para mais detalhes acesse {Log.LogBanco}.\n\n{ex.Message}\n\n{ex.InnerException.Message}", "Mais Detalhes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private async void DeletarAdiantamento(object obj)
        {
            //TODO: Colocar strings em resources
            MessageBoxResult telaApagar = MessageBoxService.Show($"Tem Certeza Que Deseja Apagar o Adiantamento Criado Em {Parcela.Adiantamento.DataString}?\nTodas as Parcelas Serão Deletadas!",
                "Deletar Adiantamento",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (telaApagar.Equals(MessageBoxResult.Yes))
            {
                try
                {
                    await daoAdiantamento.Deletar(Parcela.Adiantamento);
                    Parcelas.Remove(Parcela);
                    _dialogResult = true;
                    OnPropertyChanged("TotalParcelas");
                    MessageBoxService.Show("Adiantamento Deletado Com Sucesso!", "Mais Detalhes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBoxService.Show($"Erro ao deletar adiantamento. Para mais detalhes acesse {Log.LogBanco}.\n\n{ex.Message}\n\n{ex.InnerException.Message}", "Mais Detalhes", MessageBoxButton.OK, MessageBoxImage.Error);

                    try
                    {
                        await daoFolha.RefreshEntidade(FolhaPagamento);
                    }
                    catch (Exception ex2)
                    {
                        MessageBoxService.Show($"Erro ao dar refresh em Folha de Pagamento. Para mais detalhes acesse {Log.LogBanco}.\n\n{ex2.Message}\n\n{ex2.InnerException.Message}", "Mais Detalhes", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        public bool? ResultadoDialog()
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
            get => Bonus.Sum(s => s.Valor);
        }

        public double TotalParcelas
        {
            get => Parcelas.Sum(s => s.Valor);
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

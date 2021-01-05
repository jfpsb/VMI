using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FolhaPagamentoModel = VandaModaIntimaWpf.Model.FolhaPagamento;
using FuncionarioModel = VandaModaIntimaWpf.Model.Funcionario;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class PesquisarFolhaVM : APesquisarViewModel<FolhaPagamentoModel>
    {
        private DAOFuncionario daoFuncionario;
        private DateTime _dataEscolhida;
        private ObservableCollection<FolhaPagamentoModel> _folhaPagamentos;
        private FolhaPagamentoModel _folhaPagamento;
        private IList<FuncionarioModel> _funcionarios;

        public ICommand AbrirAdicionarAdiantamentoComando { get; set; }
        public ICommand AbrirAdicionarBonusComando { get; set; }
        public ICommand AbrirMaisDetalhesComando { get; set; }

        public PesquisarFolhaVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<FolhaPagamentoModel> abrePelaTelaPesquisaService)
            : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            //TODO: excel para folha de pagamento
            daoEntidade = new DAOFolhaPagamento(_session);
            daoFuncionario = new DAOFuncionario(_session);
            pesquisarViewModelStrategy = new PesquisarFolhaMsgVMStrategy();
            excelStrategy = new ExcelStrategy(new FolhaPagamentoExcelStrategy());

            ConsultaFuncionarios();

            if (DateTime.Now.Day > 5)
            {
                DataEscolhida = DateTime.Now.AddMonths(1);
            }
            else
            {
                DataEscolhida = DateTime.Now;
            }

            AbrirAdicionarAdiantamentoComando = new RelayCommand(AbrirAdicionarAdiantamento);
            AbrirAdicionarBonusComando = new RelayCommand(AbrirAdicionarBonus);
            AbrirMaisDetalhesComando = new RelayCommand(AbrirMaisDetalhes);
        }

        private void AbrirAdicionarBonus(object obj)
        {
            AdicionarBonusVM adicionarBonusViewModel = new AdicionarBonusVM(_session, FolhaPagamento, new MessageBoxService(), false);

            AdicionarBonus adicionarBonus = new AdicionarBonus()
            {
                DataContext = adicionarBonusViewModel
            };

            adicionarBonus.ShowDialog();

            OnPropertyChanged("TermoPesquisa");
        }

        private void AbrirMaisDetalhes(object obj)
        {
            //if (FolhaPagamento.Parcelas.Count > 0)
            //{
            //    MaisDetalhesVM maisDetalhesViewModel = new MaisDetalhesVM(_session, FolhaPagamento, new MessageBoxService());
            //    MaisDetalhes maisDetalhes = new MaisDetalhes()
            //    {
            //        DataContext = maisDetalhesViewModel
            //    };
            //    maisDetalhes.ShowDialog();

            //    OnPropertyChanged("TermoPesquisa");
            //}
        }

        private void AbrirAdicionarAdiantamento(object obj)
        {
            AdicionarAdiantamentoVM adicionarAdiantamentoViewModel = new AdicionarAdiantamentoVM(_session, FolhaPagamento.Funcionario, new MessageBoxService(), false);

            AdicionarAdiantamento adicionarAdiantamento = new AdicionarAdiantamento()
            {
                DataContext = adicionarAdiantamentoViewModel
            };

            adicionarAdiantamento.ShowDialog();

            OnPropertyChanged("TermoPesquisa");
        }

        public override async void ExportarExcel(object parameter)
        {
            SetStatusBarAguardandoExcel();
            IsThreadLocked = true;
            await new Excel<FolhaPagamentoModel>(excelStrategy).Salvar(new List<FolhaPagamentoModel>(FolhaPagamentos));
            IsThreadLocked = false;
            SetStatusBarExportadoComSucesso();
        }

        public DateTime DataEscolhida
        {
            get => _dataEscolhida;
            set
            {
                _dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
                OnPropertyChanged("TermoPesquisa");
            }
        }

        public ObservableCollection<FolhaPagamentoModel> FolhaPagamentos
        {
            get => _folhaPagamentos;
            set
            {
                _folhaPagamentos = value;
                OnPropertyChanged("FolhaPagamentos");
            }
        }

        public FolhaPagamentoModel FolhaPagamento
        {
            get => _folhaPagamento;
            set
            {
                _folhaPagamento = value;
                OnPropertyChanged("FolhaPagamento");
            }
        }

        public override bool Editavel(object parameter)
        {
            return true;
        }

        public override async void PesquisaItens(string termo)
        {
            ObservableCollection<FolhaPagamentoModel> folhas = new ObservableCollection<FolhaPagamentoModel>();
            DAOFolhaPagamento daoFolha = (DAOFolhaPagamento)daoEntidade;

            foreach (FuncionarioModel funcionario in _funcionarios)
            {
                FolhaPagamentoModel folha = await daoFolha.ListarPorMesAnoFuncionario(funcionario, DataEscolhida.Month, DataEscolhida.Year);

                if (folha == null)
                {
                    folha = new FolhaPagamentoModel
                    {
                        Id = string.Format("{0}{1}{2}", DataEscolhida.Month, DataEscolhida.Year, funcionario.Cpf),
                        Mes = DataEscolhida.Month,
                        Ano = DataEscolhida.Year,
                        Funcionario = funcionario
                    };
                }

                folhas.Add(folha);
            }

            FolhaPagamentos = folhas;
        }

        private async void ConsultaFuncionarios()
        {
            _funcionarios = await daoFuncionario.Listar<FuncionarioModel>();
        }
    }
}

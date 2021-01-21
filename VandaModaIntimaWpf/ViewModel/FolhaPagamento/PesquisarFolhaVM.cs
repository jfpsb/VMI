using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
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
        private DAOBonus daoBonus;
        private DateTime _dataEscolhida;
        private ObservableCollection<FolhaPagamentoModel> _folhaPagamentos;
        private FolhaPagamentoModel _folhaPagamento;
        private IList<FuncionarioModel> _funcionarios;

        public ICommand AbrirAdicionarAdiantamentoComando { get; set; }
        public ICommand AbrirAdicionarHoraExtraComando { get; set; }
        public ICommand AbrirAdicionarBonusComando { get; set; }
        public ICommand AbrirMaisDetalhesComando { get; set; }
        public ICommand AbrirCalculoPassagemComando { get; set; }
        public ICommand AbrirImprimirFolhaComando { get; set; }
        public ICommand AbrirVisualizarHoraExtraComando { get; set; }

        public PesquisarFolhaVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<FolhaPagamentoModel> abrePelaTelaPesquisaService)
            : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            //TODO: excel para folha de pagamento
            daoEntidade = new DAOFolhaPagamento(_session);
            daoFuncionario = new DAOFuncionario(_session);
            daoBonus = new DAOBonus(_session);
            pesquisarViewModelStrategy = new PesquisarFolhaMsgVMStrategy();
            excelStrategy = new ExcelStrategy(new FolhaPagamentoExcelStrategy());

            ConsultaFuncionarios();

            DataEscolhida = DateTime.Now;

            if (DateTime.Now.Day <= RetornaQuintoDiaUtil().Day)
            {
                DataEscolhida = DateTime.Now.AddMonths(-1);
            }

            AbrirAdicionarAdiantamentoComando = new RelayCommand(AbrirAdicionarAdiantamento);
            AbrirAdicionarBonusComando = new RelayCommand(AbrirAdicionarBonus);
            AbrirAdicionarHoraExtraComando = new RelayCommand(AbrirHoraExtra);
            AbrirMaisDetalhesComando = new RelayCommand(AbrirMaisDetalhes);
            AbrirCalculoPassagemComando = new RelayCommand(AbrirCalculoPassagem);
            AbrirImprimirFolhaComando = new RelayCommand(AbrirImprimirFolha);
            AbrirVisualizarHoraExtraComando = new RelayCommand(AbrirVisualizarHoraExtra);
        }

        private void AbrirVisualizarHoraExtra(object obj)
        {
            VisualizarHoraExtraVM visualizarHoraExtraVM = new VisualizarHoraExtraVM(DataEscolhida, new MessageBoxService(), null);
            VisualizarHoraExtra visualizarHoraExtra = new VisualizarHoraExtra
            {
                DataContext = visualizarHoraExtraVM
            };
            visualizarHoraExtra.ShowDialog();
        }

        private void AbrirImprimirFolha(object obj)
        {
            TelaRelatorioFolha telaRelatorioFolha = new TelaRelatorioFolha(FolhaPagamento);
            telaRelatorioFolha.Show();
        }

        private DateTime RetornaQuintoDiaUtil()
        {
            if (DataEscolhida.Year < 2000)
                return new DateTime(DataEscolhida.Year, DataEscolhida.Month, 5);

            if (!File.Exists($"Resources/Feriados/{DataEscolhida.Year}.json") && DataEscolhida.Year > 1999)
            {
                try
                {
                    string url = string.Format("https://api.calendario.com.br/?json=true&ano={0}&estado=MA&cidade=SAO_LUIS&token=amZwc2JfZmVsaXBlMkBob3RtYWlsLmNvbSZoYXNoPTE1NDcxMDY0NA", DataEscolhida.Year);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    WebResponse response = request.GetResponse();
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        File.WriteAllText($"Resources/Feriados/{DataEscolhida.Year}.json", reader.ReadToEnd());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            var datasFeriadosJson = File.ReadAllText($"Resources/Feriados/{DataEscolhida.Year}.json");
            var datasFeriados = JsonConvert.DeserializeObject<DataFeriado[]>(datasFeriadosJson);

            int quintoFlag = 0;

            foreach (var dia in AllDatesInMonth(DataEscolhida.Year, DataEscolhida.Month))
            {
                if (dia.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                var feriado = datasFeriados.FirstOrDefault(s => s.Date.Day == dia.Day && s.Date.Month == dia.Month);

                if (feriado != null)
                {
                    if (feriado.Type.ToLower().Equals("feriado nacional") || feriado.Type.ToLower().Equals("feriado estadual") || feriado.Type.ToLower().Equals("feriado municipal"))
                        continue;
                }

                quintoFlag++;

                if (quintoFlag == 5)
                    return dia;
            }

            return new DateTime(DataEscolhida.Year, DataEscolhida.Month, 5);
        }

        private IEnumerable<DateTime> AllDatesInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                yield return new DateTime(year, month, day);
            }
        }

        private void AbrirCalculoPassagem(object obj)
        {
            CalculoPassagemOnibusVM onibusVM = new CalculoPassagemOnibusVM(DataEscolhida);
            CalculoPassagemOnibus calculoPassagemOnibus = new CalculoPassagemOnibus()
            {
                DataContext = onibusVM
            };
            calculoPassagemOnibus.ShowDialog();
            OnPropertyChanged("TermoPesquisa");
        }

        private void AbrirHoraExtra(object obj)
        {
            AdicionarBonusVM adicionarBonusViewModel = new AdicionarBonusVM(_session, FolhaPagamento, DataEscolhida, new MessageBoxService(), false);

            AdicionarHoraExtra adicionarHoraExtra = new AdicionarHoraExtra()
            {
                DataContext = adicionarBonusViewModel
            };

            adicionarHoraExtra.ShowDialog();
            OnPropertyChanged("TermoPesquisa");
        }

        private void AbrirAdicionarBonus(object obj)
        {
            AdicionarBonusVM adicionarBonusViewModel = new AdicionarBonusVM(_session, FolhaPagamento, DataEscolhida, new MessageBoxService(), false);

            AdicionarBonus adicionarBonus = new AdicionarBonus()
            {
                DataContext = adicionarBonusViewModel
            };

            adicionarBonus.ShowDialog();

            OnPropertyChanged("TermoPesquisa");
        }

        private void AbrirMaisDetalhes(object obj)
        {
            MaisDetalhesVM maisDetalhesViewModel = new MaisDetalhesVM(_session, FolhaPagamento, new MessageBoxService());
            MaisDetalhes maisDetalhes = new MaisDetalhes()
            {
                DataContext = maisDetalhesViewModel
            };
            maisDetalhes.ShowDialog();

            OnPropertyChanged("TermoPesquisa");
        }

        private void AbrirAdicionarAdiantamento(object obj)
        {
            AdicionarAdiantamentoVM adicionarAdiantamentoViewModel = new AdicionarAdiantamentoVM(_session, DataEscolhida, FolhaPagamento.Funcionario, new MessageBoxService(), false);

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
                await _session.RefreshAsync(funcionario);

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

                folha.Bonus = await daoBonus.ListarPorFuncionarioComBonusMensal(funcionario, DataEscolhida.Month, DataEscolhida.Year);

                if (folha.Funcionario.SalarioFamilia)
                {
                    Model.Bonus salarioFamiliaBonus = new Model.Bonus()
                    {
                        Id = DateTime.Now.Ticks,
                        Funcionario = funcionario,
                        Data = DateTime.Parse(folha.MesReferencia),
                        Descricao = "SALÁRIO FAMÍLIA",
                        Valor = folha.TabelaINSS.SalarioFamilia * folha.Funcionario.NumDependentes,
                        MesReferencia = folha.Mes,
                        AnoReferencia = folha.Ano
                    };

                    folha.Bonus.Add(salarioFamiliaBonus);
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

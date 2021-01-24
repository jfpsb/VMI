using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
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
        private DAOBonusMensal daoBonusMensal;
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
        public ICommand FecharFolhaPagamentoComando { get; set; }
        public ICommand FecharFolhasAbertasComando { get; set; }

        public PesquisarFolhaVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<FolhaPagamentoModel> abrePelaTelaPesquisaService)
            : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            //TODO: excel para folha de pagamento
            daoEntidade = new DAOFolhaPagamento(_session);
            daoFuncionario = new DAOFuncionario(_session);
            daoBonusMensal = new DAOBonusMensal(_session);
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
            FecharFolhaPagamentoComando = new RelayCommand(FecharFolhaPagamento);
            FecharFolhasAbertasComando = new RelayCommand(FecharFolhasAbertas);
        }

        /// <summary>
        /// Fecha todas as folhas de pagamento abertas que estão atualmente sendo listadas.
        /// </summary>
        /// <param name="obj"></param>
        private async void FecharFolhasAbertas(object obj)
        {
            var resultMessageBox = MessageBoxService.Show($"Tem Certeza Que Deseja Fechar Todas As Folhas de Pagamento Referentes À {FolhaPagamentos[0].MesReferencia}? Essa Ação Não Pode Ser Revertida.",
                $"Fechar Folhas - {FolhaPagamentos[0].MesReferencia}", MessageBoxButton.YesNo,
                MessageBoxImage.Exclamation,
                MessageBoxResult.No);

            if (resultMessageBox == MessageBoxResult.Yes)
            {
                var folhasAbertas = FolhaPagamentos.Where(w => w.Fechada == false).ToList();

                foreach (var folhaAberta in folhasAbertas)
                {
                    folhaAberta.Fechada = true;
                }

                var result = await daoEntidade.InserirOuAtualizar(folhasAbertas);

                if (result)
                {
                    //TODO: SYNC após inserir/atualizar
                    MessageBoxService.Show("Todas As Folhas de Pagamento Foram Fechadas Com Sucesso!");
                    OnPropertyChanged("TermoPesquisa");
                }
                else
                {
                    MessageBoxService.Show("Erro Ao Fechar Folhas de Pagamento!");
                }
            }
        }

        /// <summary>
        /// Fecha folha de pagamento atualmente selecionada.
        /// </summary>
        /// <param name="obj"></param>
        private async void FecharFolhaPagamento(object obj)
        {
            var resultMessageBox = MessageBoxService.Show($"Tem Certeza Que Deseja Fechar a Folha de Pagamento de {FolhaPagamento.Funcionario.Nome} Referente À {FolhaPagamento.MesReferencia}? Essa Ação Não Pode Ser Revertida.",
                $"Fechar Folha - {FolhaPagamento.Funcionario.Nome} - {FolhaPagamento.MesReferencia}", MessageBoxButton.YesNo,
                MessageBoxImage.Exclamation,
                MessageBoxResult.No);

            if (resultMessageBox == MessageBoxResult.Yes)
            {
                FolhaPagamento.Fechada = true;

                var result = await daoEntidade.InserirOuAtualizar(FolhaPagamento);

                if (result)
                {
                    //TODO: SYNC após inserir/atualizar
                    MessageBoxService.Show("Folha de Pagamento Fechada Com Sucesso!");
                    OnPropertyChanged("TermoPesquisa");
                }
                else
                {
                    MessageBoxService.Show("Erro Ao Fechar Folha de Pagamento!");
                }
            }
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

            foreach (var dia in DateTimeUtil.RetornaDiasEmMes(DataEscolhida.Year, DataEscolhida.Month))
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
                        Mes = DataEscolhida.Month,
                        Ano = DataEscolhida.Year,
                        Funcionario = funcionario,
                        BaseCalculo = funcionario.Salario
                    };
                }

                //Lista todos os bônus (inclusive bônus cancelados)
                folha.Bonus = await daoBonus.ListarPorFuncionario(funcionario, DataEscolhida.Month, DataEscolhida.Year);

                if (!folha.Fechada)
                {
                    //Lista os bônus mensais do funcionário
                    var bonusMensais = await daoBonusMensal.ListarBonusMensais(funcionario);

                    foreach (var bonusMensal in bonusMensais)
                    {
                        //Checa se o bônus mensal já existe na lista de bônus de funcionário
                        var bonusJaExiste = folha.Bonus.Any(a => a.Descricao.Equals(bonusMensal.Descricao));

                        if (bonusJaExiste)
                            continue;

                        //Se não existe cria o bônus
                        Bonus bonus = new Bonus
                        {
                            Funcionario = funcionario,
                            Data = new DateTime(folha.Ano, folha.Mes, 1),
                            Descricao = bonusMensal.Descricao,
                            Valor = bonusMensal.Valor,
                            MesReferencia = folha.Mes,
                            AnoReferencia = folha.Ano,
                            BaseCalculo = funcionario.Salario,
                            BonusMensal = true
                        };

                        folha.Bonus.Add(bonus);
                    }

                    if (folha.Funcionario.SalarioFamilia)
                    {
                        Model.Bonus salarioFamiliaBonus = new Model.Bonus()
                        {
                            Funcionario = funcionario,
                            Data = DateTime.Parse(folha.MesReferencia),
                            Descricao = "SALÁRIO FAMÍLIA",
                            Valor = folha.TabelaINSS.SalarioFamilia * folha.Funcionario.NumDependentes,
                            MesReferencia = folha.Mes,
                            AnoReferencia = folha.Ano
                        };

                        folha.Bonus.Add(salarioFamiliaBonus);
                    }
                }

                //Depois da checagem acima, removo os bônus cancelados da listagem
                folha.Bonus = folha.Bonus.Where(w => w.BonusCancelado == false).ToList();

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

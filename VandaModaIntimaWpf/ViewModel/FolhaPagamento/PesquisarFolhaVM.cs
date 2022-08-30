using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.View.Interfaces;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.CrystalReports;
using VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Funcionario;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class PesquisarFolhaVM : APesquisarViewModel<Model.FolhaPagamento>
    {
        private DAOFuncionario daoFuncionario;
        private DAOBonus daoBonus;
        private DAOBonusMensal daoBonusMensal;
        private DAOParcela daoParcela;
        private DAODespesa daoDespesa;
        private DAOTipoDespesa daoTipoDespesa;
        private DateTime _dataEscolhida;
        private ObservableCollection<Model.FolhaPagamento> _folhaPagamentos;
        private Model.FolhaPagamento _folhaPagamento;
        private IList<Model.Funcionario> _funcionarios;
        private double _totalEmPassagem;
        private double _totalEmAlimentacao;
        private double _totalEmMeta;
        private ICrystalReportsParaPDF<Model.FolhaPagamento> crystalReportsParaPDF;

        public ICommand AbrirAdicionarAdiantamentoComando { get; set; }
        public ICommand AbrirAdicionarHoraExtraComando { get; set; }
        public ICommand AbrirAdicionarBonusComando { get; set; }
        public ICommand AbrirMaisDetalhesComando { get; set; }
        public ICommand AbrirCalculoPassagemComando { get; set; }
        public ICommand AbrirCalculoAlmocoComando { get; set; }
        public ICommand AbrirImprimirFolhaComando { get; set; }
        public ICommand AbrirVisualizarHoraExtraFaltasComando { get; set; }
        public ICommand FecharFolhaPagamentoComando { get; set; }
        public ICommand FecharFolhasAbertasComando { get; set; }
        public ICommand AbrirAdicionarSalarioLiquidoComando { get; set; }
        public ICommand AbrirDadosBancariosComando { get; set; }
        public ICommand AbrirAdicionarObservacaoComando { get; set; }
        public ICommand ExportarFolhasParaPDFComando { get; set; }
        public ICommand AdicionarMetaIndividualComando { get; set; }
        public ICommand AbrirAdicionarTotalComando { get; set; }
        public ICommand GerarUltimaFolhaPagamentoComando { get; set; }
        public ICommand AbrirAdicionarFaltasComando { get; set; }
        public ICommand AdicionarValoresDespesaComando { get; set; }

        public PesquisarFolhaVM()
        {
            //TODO: excel para folha de pagamento
            daoEntidade = new DAOFolhaPagamento(_session);
            daoFuncionario = new DAOFuncionario(_session);
            daoBonusMensal = new DAOBonusMensal(_session);
            daoTipoDespesa = new DAOTipoDespesa(_session);
            daoDespesa = new DAODespesa(_session);
            daoParcela = new DAOParcela(_session);
            daoBonus = new DAOBonus(_session);
            pesquisarViewModelStrategy = new PesquisarFolhaMsgVMStrategy();
            excelStrategy = new FolhaPagamentoExcelStrategy();
            FolhaPagamentos = new ObservableCollection<Model.FolhaPagamento>();
            crystalReportsParaPDF = new CRFolhaPagamentoParaPDF(_session);
            cancellationTokenSource = new CancellationTokenSource();

            GetFuncionarios();

            DataEscolhida = DateTime.Now;

            if (DateTime.Now.Day <= DateTimeUtil.RetornaDataUtil(5, DataEscolhida.Month, DataEscolhida.Year).Day)
            {
                DataEscolhida = DateTime.Now.AddMonths(-1);
            }

            AbrirAdicionarAdiantamentoComando = new RelayCommand(AbrirAdicionarAdiantamento);
            AbrirAdicionarBonusComando = new RelayCommand(AbrirAdicionarBonus);
            AbrirAdicionarHoraExtraComando = new RelayCommand(AbrirHoraExtra);
            AbrirMaisDetalhesComando = new RelayCommand(AbrirMaisDetalhes);
            AbrirCalculoPassagemComando = new RelayCommand(AbrirCalculoPassagem);
            AbrirCalculoAlmocoComando = new RelayCommand(AbrirCalculoAlmoco);
            AbrirImprimirFolhaComando = new RelayCommand(AbrirImprimirFolha);
            AbrirVisualizarHoraExtraFaltasComando = new RelayCommand(AbrirVisualizarHoraExtraFaltas);
            FecharFolhaPagamentoComando = new RelayCommand(FecharFolhaPagamento);
            FecharFolhasAbertasComando = new RelayCommand(FecharFolhasAbertas);
            AbrirAdicionarSalarioLiquidoComando = new RelayCommand(AbrirAdicionarSalarioLiquido);
            AbrirDadosBancariosComando = new RelayCommand(AbrirDadosBancarios);
            AbrirAdicionarObservacaoComando = new RelayCommand(AbrirAdicionarObservacao);
            ExportarFolhasParaPDFComando = new RelayCommand(ExportarFolhasParaPDF);
            AdicionarMetaIndividualComando = new RelayCommand(AdicionarMeta);
            AbrirAdicionarTotalComando = new RelayCommand(AbrirAdicionarTotal);
            GerarUltimaFolhaPagamentoComando = new RelayCommand(GerarUltimaFolhaPagamento);
            AbrirAdicionarFaltasComando = new RelayCommand(AbrirAdicionarFaltas);
            AdicionarValoresDespesaComando = new RelayCommand(AdicionarValoresDespesa);
        }

        private async void AdicionarValoresDespesa(object obj)
        {
            IList<Model.Despesa> despesas = new List<Model.Despesa>();
            var folhasPorLojaSoma = FolhaPagamentos.GroupBy(g => g.Funcionario.Loja)
                .Select(g => new { Loja = g.Key, Soma = g.Sum(s => s.ValorATransferir), Vencimento = g.First().Vencimento });

            foreach (var item in folhasPorLojaSoma)
            {
                var despesa = new Model.Despesa
                {
                    TipoDespesa = await daoTipoDespesa.RetornaTipoDespesaEmpresarial(),
                    Loja = item.Loja,
                    Valor = item.Soma,
                    DataVencimento = item.Vencimento,
                    Data = DateTime.Now,
                    Descricao = "SALÁRIOS DE FUNCIONÁRIOS"
                };

                despesas.Add(despesa);
            }

            try
            {
                await daoDespesa.Inserir(despesas);
                _messageBoxService.Show("Valores De Salários De Funcionários Foram Adicionados Em Despesas Com Sucesso!", "Adicionar Salários De Funcionários Em Despesas", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _messageBoxService.Show("Erro ao adicionar valores de salários de funcionários!" +
                    $"Para mais detalhes acesse {Log.LogBanco}.\n\n" +
                    $"{ex.Message}\n\n{ex.InnerException.Message}", "Adicionar Salários De Funcionários Em Despesas", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AbrirAdicionarFaltas(object obj)
        {
            _windowService.ShowDialog(new AdicionarFaltasVM(_session, FolhaPagamento), (result, viewModel) =>
            {
                OnPropertyChanged("TermoPesquisa");
            });
        }

        private async void GerarUltimaFolhaPagamento(object obj)
        {
            var resultMessageBox = _messageBoxService.Show($"Tem Certeza Que Deseja Gerar A Última Folha De Pagamento Do(a) Funcionário(a) {FolhaPagamento.Funcionario.Nome}?\n\nA Última Folha Irá Listar Todas As Parcelas Ainda Não Pagas De Adiantamentos.",
                $"Gerar Última Folha De Pagamento - {FolhaPagamento.Funcionario.Nome}", MessageBoxButton.YesNo,
                MessageBoxImage.Exclamation,
                MessageBoxResult.No);

            if (resultMessageBox == MessageBoxResult.Yes)
            {
                var parcelas = await daoParcela.ListarPorFuncionarioNaoPagas(FolhaPagamento.Funcionario);

                FolhaPagamento.Parcelas = parcelas;

                TelaRelatorioFolha telaRelatorioFolha = new TelaRelatorioFolha(_session, FolhaPagamento);
                telaRelatorioFolha.ShowDialog();
                OnPropertyChanged("TermoPesquisa");
            }
        }
        private void AbrirAdicionarTotal(object obj)
        {
            _windowService.ShowDialog(new AdicionarTotalVendidoVM(_session, FolhaPagamento), (result, viewModel) =>
            {
                OnPropertyChanged("TermoPesquisa");
            });
        }
        private void AdicionarMeta(object obj)
        {
            _windowService.ShowDialog(new AdicionarMetaIndividualVM(_session, FolhaPagamentos, _messageBoxService), (result, viewModel) =>
            {
                OnPropertyChanged("TermoPesquisa");
            });
        }
        private async void ExportarFolhasParaPDF(object parameter)
        {
            try
            {
                if (parameter == null)
                    throw new Exception($"Parâmetro de comando não configurado para ExportarExcel em Pesquisar Folha De Pagamento.");

                var folderBrowserDialog = parameter as IFolderBrowserDialog;
                string caminhoPasta = folderBrowserDialog.OpenFolderBrowserDialog();

                if (caminhoPasta != null)
                {
                    CancellationToken token = cancellationTokenSource.Token;
                    VisibilidadeStatusBar = Visibility.Visible;
                    Task task = crystalReportsParaPDF.Salvar(caminhoPasta, FolhaPagamentos, setValorBarraProgresso, setDescricaoBarraProgresso, setIsIndefinidaBarraProgresso, token);

                    try
                    {
                        await task;
                    }
                    catch (OperationCanceledException)
                    {
                        _messageBoxService.Show("Exportação para PDF foi cancelada pelo usuário");
                        cancellationTokenSource.Dispose();
                        cancellationTokenSource = new CancellationTokenSource();
                    }

                    await task.ContinueWith(t =>
                    {
                        VisibilidadeStatusBar = Visibility.Collapsed;
                        if (!task.IsCanceled)
                            _messageBoxService.Show("Folhas foram exportadas em PDF com sucesso");
                    });
                }
            }
            catch (OperationCanceledException)
            {
                _messageBoxService.Show("Exportação para PDF foi cancelada pela usuário");
                cancellationTokenSource.Dispose();
                cancellationTokenSource = new CancellationTokenSource();
            }
            catch (Exception ex)
            {
                _messageBoxService.Show(ex.Message);
            }
        }
        private void AbrirAdicionarObservacao(object obj)
        {
            _windowService.ShowDialog(new AdicionarObservacaoFolhaVM(_session, FolhaPagamento, new MessageBoxService()), (result, viewModel) =>
            {
                OnPropertyChanged("TermoPesquisa");
            });
        }
        private void AbrirCalculoAlmoco(object obj)
        {
            _windowService.ShowDialog(new CalculoDeBonusMensalPorDiaVM(_session, DataEscolhida, new CalculoDeAlmoco()), (result, viewModel) =>
            {
                OnPropertyChanged("TermoPesquisa");
            });
        }
        private void AbrirDadosBancarios(object obj)
        {
            _windowService.Show(new VisualizarDadosBancariosVM(FolhaPagamento.Funcionario), null);
        }
        private void AbrirAdicionarSalarioLiquido(object obj)
        {
            _windowService.ShowDialog(new AdicionarSalarioLiquidoVM(_session, _folhaPagamento), (result, viewModel) =>
            {
                OnPropertyChanged("TermoPesquisa");
            });
        }

        /// <summary>
        /// Fecha todas as folhas de pagamento abertas que estão atualmente sendo listadas.
        /// </summary>
        /// <param name="obj"></param>
        private async void FecharFolhasAbertas(object obj)
        {
            var resultMessageBox = _messageBoxService.Show($"Tem Certeza Que Deseja Fechar Todas As Folhas de Pagamento Referentes À {FolhaPagamentos[0].MesReferencia}? Essa Ação Não Pode Ser Revertida.",
                $"Fechar Folhas - {FolhaPagamentos[0].MesReferencia}", MessageBoxButton.YesNo,
                MessageBoxImage.Exclamation,
                MessageBoxResult.No);

            if (resultMessageBox == MessageBoxResult.Yes)
            {
                var folhasAbertas = FolhaPagamentos.Where(w => w.Fechada == false).ToList();
                var parcelasEmAberto = folhasAbertas.SelectMany(sm => sm.Parcelas).ToList();
                var bonusDeFolhas = folhasAbertas.SelectMany(sm => sm.Bonus).ToList();

                foreach (var folhaAberta in folhasAbertas)
                {
                    folhaAberta.Fechada = true;
                }

                foreach (var parcela in parcelasEmAberto)
                {
                    parcela.Paga = true;
                }

                var dao = daoEntidade as DAOFolhaPagamento;
                var result = await dao.FecharFolhasDePagamento(folhasAbertas, parcelasEmAberto, bonusDeFolhas);

                if (result)
                {
                    //TODO: SYNC após inserir/atualizar
                    _messageBoxService.Show("Todas As Folhas de Pagamento Foram Fechadas Com Sucesso!");
                    OnPropertyChanged("TermoPesquisa");
                }
                else
                {
                    _messageBoxService.Show("Erro Ao Fechar Folhas de Pagamento!");
                }
            }
        }

        /// <summary>
        /// Fecha folha de pagamento atualmente selecionada.
        /// </summary>
        /// <param name="obj"></param>
        private async void FecharFolhaPagamento(object obj)
        {
            var resultMessageBox = _messageBoxService.Show($"Tem Certeza Que Deseja Fechar a Folha de Pagamento de {FolhaPagamento.Funcionario.Nome} Referente À {FolhaPagamento.MesReferencia}? Essa Ação Não Pode Ser Revertida.",
                $"Fechar Folha - {FolhaPagamento.Funcionario.Nome} - {FolhaPagamento.MesReferencia}", MessageBoxButton.YesNo,
                MessageBoxImage.Exclamation,
                MessageBoxResult.No);

            if (resultMessageBox == MessageBoxResult.Yes)
            {
                FolhaPagamento.Fechada = true;

                foreach (var parcela in FolhaPagamento.Parcelas)
                {
                    parcela.Paga = true;
                }


                try
                {
                    var dao = daoEntidade as DAOFolhaPagamento;
                    await dao.FecharFolhaDePagamento(FolhaPagamento);
                    _messageBoxService.Show("Folha de Pagamento Fechada Com Sucesso!", "Pesquisa De Folha De Pagamento",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    OnPropertyChanged("TermoPesquisa");
                }
                catch (Exception ex)
                {
                    _messageBoxService.Show($"Erro ao fechar folha de pagamento. Para mais detalhes acesse {Log.LogBanco}.\n\n" +
                        $"{ex.Message}\n\n{ex.InnerException.Message}", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AbrirVisualizarHoraExtraFaltas(object obj)
        {
            _messageBoxService.Show("CERTIFIQUE-SE DE FECHAR AS FOLHAS DE PAGAMENTO ANTES DE EXPORTAR O RELATÓRIO DE HORAS EXTRAS/FALTAS/COMISSÕES PARA O CONTADOR." +
                "\n\nEXPORTAR ESTE RELATÓRIO SEM FECHAR AS FOLHAS PREVIAMENTE IRÁ GERAR RELATÓRIO INCORRETO, COM INFORMAÇÕES FALTANDO.", "Horas Extras/Faltas/Comissões", MessageBoxButton.OK, MessageBoxImage.Warning);
            _windowService.Show(new VisualizarHoraExtraFaltasVM(DataEscolhida), null);
        }

        private void AbrirImprimirFolha(object obj)
        {
            TelaRelatorioFolha telaRelatorioFolha = new TelaRelatorioFolha(_session, FolhaPagamento);
            telaRelatorioFolha.Show();
        }

        private void AbrirCalculoPassagem(object obj)
        {
            _windowService.ShowDialog(new CalculoDeBonusMensalPorDiaVM(_session, DataEscolhida, new CalculoDePassagem()), (result, viewModel) =>
            {
                OnPropertyChanged("TermoPesquisa");
            });
        }

        private void AbrirHoraExtra(object obj)
        {
            _windowService.ShowDialog(new AdicionarHoraExtraVM(_session, FolhaPagamento), (result, viewModel) =>
            {
                if (result == true)
                {
                    OnPropertyChanged("TermoPesquisa");
                }
            });
        }

        private void AbrirAdicionarBonus(object obj)
        {
            _windowService.ShowDialog(new AdicionarBonusVM(_session, FolhaPagamento, DataEscolhida), (result, viewModel) =>
            {
                if (result == true)
                {
                    OnPropertyChanged("TermoPesquisa");
                }
            });
        }

        private void AbrirMaisDetalhes(object obj)
        {
            _windowService.ShowDialog(new MaisDetalhesVM(_session, FolhaPagamento), (result, viewModel) =>
            {
                if (result == true)
                {
                    OnPropertyChanged("TermoPesquisa");
                }
            });
        }

        private void AbrirAdicionarAdiantamento(object obj)
        {
            _windowService.ShowDialog(new AdicionarAdiantamentoVM(_session, DataEscolhida, FolhaPagamento.Funcionario), (result, viewModel) =>
            {
                if (result == true)
                {
                    OnPropertyChanged("TermoPesquisa");
                }
            });
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

        public ObservableCollection<Model.FolhaPagamento> FolhaPagamentos
        {
            get => _folhaPagamentos;
            set
            {
                _folhaPagamentos = value;
                OnPropertyChanged("FolhaPagamentos");
                OnPropertyChanged("TotalAPagar");
            }
        }

        public Model.FolhaPagamento FolhaPagamento
        {
            get => _folhaPagamento;
            set
            {
                _folhaPagamento = value;
                OnPropertyChanged("FolhaPagamento");
            }
        }
        public double TotalAPagar
        {
            get => FolhaPagamentos.Select(s => s.ValorATransferir).Sum();
        }
        public double TotalEmPassagem
        {
            get => _totalEmPassagem;
            set
            {
                _totalEmPassagem = value;
                OnPropertyChanged("TotalEmPassagem");
            }
        }
        public double TotalEmAlimentacao
        {
            get => _totalEmAlimentacao;
            set
            {
                _totalEmAlimentacao = value;
                OnPropertyChanged("TotalEmAlimentacao");
            }
        }

        public double TotalEmMeta
        {
            get => _totalEmMeta;
            set
            {
                _totalEmMeta = value;
                OnPropertyChanged("TotalEmMeta");
            }
        }

        public override bool Editavel(object parameter)
        {
            return true;
        }

        public override async Task PesquisaItens(string termo)
        {
            ObservableCollection<Model.FolhaPagamento> folhas = new ObservableCollection<Model.FolhaPagamento>();
            DAOFolhaPagamento daoFolha = (DAOFolhaPagamento)daoEntidade;

            foreach (Model.Funcionario funcionario in _funcionarios)
            {
                Model.FolhaPagamento folha = await daoFolha.ListarPorMesAnoFuncionario(funcionario, DataEscolhida.Month, DataEscolhida.Year);

                if (folha == null)
                {
                    if (DataEscolhida < funcionario.Admissao)
                        continue;

                    if (DataEscolhida > funcionario.Demissao)
                        continue;

                    folha = new Model.FolhaPagamento
                    {
                        Mes = DataEscolhida.Month,
                        Ano = DataEscolhida.Year,
                        Funcionario = funcionario
                    };
                }

                //Lista as parcelas somente se folha já existe
                var parcelas = await daoParcela.ListarPorFuncionarioMesAno(folha.Funcionario, folha.Mes, folha.Ano);
                folha.Parcelas = parcelas;

                //Lista todos os bônus do funcionário (inclusive bônus cancelados)
                folha.Bonus = await daoBonus.ListarPorFuncionario(funcionario, DataEscolhida.Month, DataEscolhida.Year);

                //Adiciona bônus
                //Só é feito em folhas abertas porque nas fechadas os bônus mensais e de meta já estão inseridos no BD
                if (!folha.Fechada)
                {
                    //Lista todos os bônus mensais do funcionário
                    var bonusMensais = await daoBonusMensal.ListarPorFuncionario(funcionario);

                    if (bonusMensais != null && bonusMensais.Count > 0)
                    {
                        foreach (var bonusMensal in bonusMensais)
                        {
                            //Checa se o bônus mensal já existe na lista de bônus de funcionário
                            var bonusJaInserido = folha.Bonus.Count > 0 && folha.Bonus.Any(a => a.Descricao.Equals(bonusMensal.Descricao));

                            if (bonusJaInserido)
                                continue;

                            //Se não existe cria e adiciona o bônus
                            Bonus bonus = new Bonus
                            {
                                Funcionario = funcionario,
                                Data = new DateTime(folha.Ano, folha.Mes, 1),
                                Descricao = bonusMensal.Descricao,
                                Valor = bonusMensal.Valor,
                                MesReferencia = folha.Mes,
                                AnoReferencia = folha.Ano,
                                BonusMensal = true,
                                PagoEmFolha = bonusMensal.PagoEmFolha
                            };

                            folha.Bonus.Add(bonus);
                        }
                    }

                    //Insere bônus de meta se houver
                    if (folha.ValorDoBonusDeMeta > 0)
                    {
                        var mesFolha = new DateTime(folha.Ano, folha.Mes, 1);
                        Bonus bonus = new Bonus
                        {
                            Funcionario = funcionario,
                            Data = new DateTime(folha.Ano, folha.Mes, DateTime.DaysInMonth(folha.Ano, folha.Mes)),
                            Descricao = $"COMISSÃO DE VENDA - {mesFolha.ToString("MMMM", CultureInfo.GetCultureInfo("pt-BR"))}",
                            Valor = folha.ValorDoBonusDeMeta,
                            MesReferencia = folha.Mes,
                            AnoReferencia = folha.Ano
                        };

                        folha.Bonus.Add(bonus);
                    }
                }

                //Depois da checagem acima, removo os bônus cancelados da listagem
                if (folha.Bonus != null)
                    folha.Bonus = folha.Bonus.Where(w => w.BonusCancelado == false).ToList();

                folhas.Add(folha);
            }

            TotalEmPassagem = await daoBonus.SomaPassagemPorMesAno(DataEscolhida);
            TotalEmAlimentacao = await daoBonus.SomaAlimentacaoPorMesAno(DataEscolhida);
            //Bonus de meta não são salvos até que a folha seja fechada então uso linq com a coleção atual para conseguir o valor e não do banco de dados
            TotalEmMeta = folhas.SelectMany(sm => sm.Bonus).Where(w => w.Descricao.Contains("META")).Sum(s => s.Valor);

            FolhaPagamentos = folhas;
        }

        private async void GetFuncionarios()
        {
            _funcionarios = await daoFuncionario.Listar();
        }

        protected override WorksheetContainer<Model.FolhaPagamento>[] GetWorksheetContainers()
        {
            var worksheets = new WorksheetContainer<Model.FolhaPagamento>[1];
            worksheets[0] = new WorksheetContainer<Model.FolhaPagamento>()
            {
                Nome = "Folhas De Pagamento",
                Lista = Entidades.Select(s => s.Entidade).ToList()
            };

            return worksheets;
        }

        public override object GetCadastrarViewModel()
        {
            throw new NotImplementedException();
        }

        public override object GetEditarViewModel()
        {
            throw new NotImplementedException();
        }
    }
}

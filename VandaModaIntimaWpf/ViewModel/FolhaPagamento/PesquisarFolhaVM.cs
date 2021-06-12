using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.View.FolhaPagamento.Relatorios;
using VandaModaIntimaWpf.View.Funcionario;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using VandaModaIntimaWpf.ViewModel.DataSets;
using VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia;
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
        private DAOParcela daoParcela;
        private DAOHoraExtra daoHoraExtra;
        private DateTime _dataEscolhida;
        private ObservableCollection<FolhaPagamentoModel> _folhaPagamentos;
        private FolhaPagamentoModel _folhaPagamento;
        private IList<FuncionarioModel> _funcionarios;
        private IFileDialogService _fileDialogService;
        private string caminhoFolhaPagamentoVMI = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Vanda Moda Íntima", "Folha De Pagamento");
        private IProgress<double> progressoValorProgresso;
        private IProgress<string> progressoDescricaoProgesso;

        //Propriedades de Tela De Progresso
        private string _tituloTelaProgresso;
        private string _descricaoBarraProgresso;
        private double _valorBarraProgresso;

        public ICommand AbrirAdicionarAdiantamentoComando { get; set; }
        public ICommand AbrirAdicionarHoraExtraComando { get; set; }
        public ICommand AbrirAdicionarBonusComando { get; set; }
        public ICommand AbrirMaisDetalhesComando { get; set; }
        public ICommand AbrirCalculoPassagemComando { get; set; }
        public ICommand AbrirCalculoAlmocoComando { get; set; }
        public ICommand AbrirImprimirFolhaComando { get; set; }
        public ICommand AbrirVisualizarHoraExtraComando { get; set; }
        public ICommand FecharFolhaPagamentoComando { get; set; }
        public ICommand FecharFolhasAbertasComando { get; set; }
        public ICommand AbrirAdicionarSalarioLiquidoComando { get; set; }
        public ICommand AbrirDadosBancariosComando { get; set; }
        public ICommand AbrirAdicionarObservacaoComando { get; set; }
        public ICommand ExportarFolhasParaPDFComando { get; set; }
        public ICommand AdicionarMetaIndividualComando { get; set; }
        public ICommand AbrirAdicionarTotalComando { get; set; }
        public ICommand GerarUltimaFolhaPagamentoComando { get; set; }

        public PesquisarFolhaVM(IMessageBoxService messageBoxService, IFileDialogService fileDialogService, IAbrePelaTelaPesquisaService<FolhaPagamentoModel> abrePelaTelaPesquisaService)
            : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            //TODO: excel para folha de pagamento
            daoEntidade = new DAOFolhaPagamento(_session);
            daoFuncionario = new DAOFuncionario(_session);
            daoBonusMensal = new DAOBonusMensal(_session);
            daoParcela = new DAOParcela(_session);
            daoBonus = new DAOBonus(_session);
            daoHoraExtra = new DAOHoraExtra(_session);
            pesquisarViewModelStrategy = new PesquisarFolhaMsgVMStrategy();
            excelStrategy = new ExcelStrategy(new FolhaPagamentoExcelStrategy());
            _fileDialogService = fileDialogService;

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
            AbrirVisualizarHoraExtraComando = new RelayCommand(AbrirVisualizarHoraExtra);
            FecharFolhaPagamentoComando = new RelayCommand(FecharFolhaPagamento);
            FecharFolhasAbertasComando = new RelayCommand(FecharFolhasAbertas);
            AbrirAdicionarSalarioLiquidoComando = new RelayCommand(AbrirAdicionarSalarioLiquido);
            AbrirDadosBancariosComando = new RelayCommand(AbrirDadosBancarios);
            AbrirAdicionarObservacaoComando = new RelayCommand(AbrirAdicionarObservacao);
            ExportarFolhasParaPDFComando = new RelayCommand(ExportarFolhasParaPDF);
            AdicionarMetaIndividualComando = new RelayCommand(AdicionarMeta);
            AbrirAdicionarTotalComando = new RelayCommand(AbrirAdicionarTotal);
            GerarUltimaFolhaPagamentoComando = new RelayCommand(GerarUltimaFolhaPagamento);

            progressoValorProgresso = new Progress<double>(valor => { ValorBarraProgresso = valor; });
            progressoDescricaoProgesso = new Progress<string>(descricao => { DescricaoBarraProgresso = descricao; });
        }

        private async void GerarUltimaFolhaPagamento(object obj)
        {
            var resultMessageBox = MessageBoxService.Show($"Tem Certeza Que Deseja Gerar A Última Folha De Pagamento Do(a) Funcionário(a) {FolhaPagamento.Funcionario.Nome}?\n\nA Última Folha Irá Listar Todas As Parcelas Ainda Não Pagas De Adiantamentos.",
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
            AdicionarTotalVendidoVM viewModel = new AdicionarTotalVendidoVM(_session, FolhaPagamento, new MessageBoxService(), false);
            AdicionarTotalVendido view = new AdicionarTotalVendido
            {
                DataContext = viewModel
            };
            view.ShowDialog();
            OnPropertyChanged("TermoPesquisa");
        }

        private void AdicionarMeta(object obj)
        {
            AdicionarMetaIndividualVM viewModel = new AdicionarMetaIndividualVM(_session, FolhaPagamentos, new MessageBoxService());
            AdicionarMetaIndividual view = new AdicionarMetaIndividual
            {
                DataContext = viewModel
            };
            view.ShowDialog();
            OnPropertyChanged("TermoPesquisa");
        }

        private async void ExportarFolhasParaPDF(object obj)
        {
            string caminhoPasta = _fileDialogService.ShowFolderBrowserDialog();
            TelaDeProgresso telaDeProgresso = new TelaDeProgresso()
            {
                DataContext = this
            };
            telaDeProgresso.Show();
            await Task.Run(() => ProcessaFolhasParaPDF(progressoValorProgresso, progressoDescricaoProgesso, caminhoPasta));
        }

        private async void ProcessaFolhasParaPDF(IProgress<double> progressoValor, IProgress<string> progressoDescricao, string caminhoPasta)
        {
            //TODO: este código está repetido em TelaRelatorioFolha.cs
            double incremento = 100.0 / FolhaPagamentos.Count;
            double progressoAtual = 0;
            TituloTelaProgresso = "Exportando Folhas De Pagamento Para PDF";

            if (!string.IsNullOrEmpty(caminhoPasta))
            {
                try
                {
                    FolhaPagamentoDataSet folhaPagamentoDataSet = new FolhaPagamentoDataSet();
                    BonusDataSet bonusDataSet = new BonusDataSet();
                    ParcelaDataSet parcelaDataSet = new ParcelaDataSet();

                    progressoDescricao.Report("Iniciando Exportação De Folhas De Pagamento Para PDF");

                    foreach (var folha in FolhaPagamentos)
                    {
                        progressoValor.Report(progressoAtual);

                        folhaPagamentoDataSet.Clear();
                        bonusDataSet.Clear();
                        parcelaDataSet.Clear();

                        progressoDescricao.Report($"Gerando Folha de {folha.Funcionario.Nome}");

                        int i = 0;
                        foreach (var bonus in folha.Bonus)
                        {
                            var brow = bonusDataSet.Bonus.NewBonusRow();

                            //Os bônus mensais que ainda não foram salvos tem Id 0, se tentar adicionar dois items com mesma Id dá erro.
                            //Então uso essa variável i apenas para setar uma Id diferente para cada bônus
                            brow.id = i++.ToString();
                            brow.data = bonus.DataString;
                            brow.descricao = bonus.Descricao;
                            brow.valor = bonus.Valor.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                            brow.total_bonus = folha.TotalBonus.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));

                            bonusDataSet.Bonus.AddBonusRow(brow);
                        }

                        foreach (var parcela in folha.Parcelas)
                        {
                            var prow = parcelaDataSet.Parcela.NewParcelaRow();
                            prow.id = parcela.Id.ToString();
                            prow.numero = parcela.Numero.ToString();
                            prow.valor = parcela.Valor.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                            prow.data_adiantamento = parcela.Adiantamento.DataString;
                            prow.numero_com_total = parcela.NumeroComTotal;
                            prow.total_adiantamentos = folha.TotalAdiantamentos.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                            prow.descricao = parcela.Adiantamento.Descricao;

                            parcelaDataSet.Parcela.AddParcelaRow(prow);
                        }

                        var parcelasNaoPagas = await daoParcela.ListarPorFuncionarioNaoPagas(folha.Funcionario);

                        var fprow = folhaPagamentoDataSet.FolhaPagamento.NewFolhaPagamentoRow();
                        fprow.mes = folha.Mes.ToString();
                        fprow.ano = folha.Ano.ToString();
                        fprow.mesreferencia = folha.MesReferencia;
                        fprow.vencimento = folha.Vencimento.ToString("dd/MM/yyyy");
                        fprow.funcionario = folha.Funcionario.Nome;
                        fprow.valor_a_transferir = folha.ValorATransferir.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                        fprow.salario_liquido = folha.SalarioLiquido.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                        fprow.observacao = folha.Observacao;
                        fprow.valordameta = folha.MetaDeVenda.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                        fprow.totalvendido = folha.TotalVendido.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                        fprow.restanteadiantamento = parcelasNaoPagas.Sum(s => s.Valor).ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));

                        var calendarioPassagem = Path.Combine(caminhoFolhaPagamentoVMI, folha.Funcionario.Nome, folha.Ano.ToString(), folha.Mes.ToString(), "CalendarioPassagem.png");
                        var calendarioAlimentacao = Path.Combine(caminhoFolhaPagamentoVMI, folha.Funcionario.Nome, folha.Ano.ToString(), folha.Mes.ToString(), "CalendarioAlimentacao.png");
                        fprow.calendariopassagem = calendarioPassagem;
                        fprow.calendarioalimentacao = calendarioAlimentacao;

                        fprow.horaextra100 = "00:00";
                        fprow.horaextra55 = "00:00";

                        var horasExtras = daoHoraExtra.ListarPorAnoMesFuncionario(folha.Ano, folha.Mes, folha.Funcionario).Result;
                        var he100 = horasExtras.Where(w => w.TipoHoraExtra.Descricao.Contains("100%")).SingleOrDefault();
                        var he55 = horasExtras.Where(w => w.TipoHoraExtra.Descricao.Contains("055%")).SingleOrDefault();

                        if (he100 != null)
                        {
                            fprow.horaextra100 = he100.HorasTimeSpan.ToString("hh\\:mm");
                        }

                        if (he55 != null)
                        {
                            fprow.horaextra55 = he55.HorasTimeSpan.ToString("hh\\:mm");
                        }

                        folhaPagamentoDataSet.FolhaPagamento.AddFolhaPagamentoRow(fprow);

                        var report = new RelatorioFolhaPagamento();
                        report.Load("/View/FolhaPagamento/Relatorios/RelatorioFolhaPagamento.rpt");
                        report.DetailSection3.SectionFormat.EnableSuppress = !folha.Funcionario.RecebePassagem || !(File.Exists(calendarioPassagem) && File.Exists(calendarioAlimentacao));
                        report.Subreports[0].SetDataSource(bonusDataSet);
                        report.Subreports[1].SetDataSource(parcelaDataSet);
                        report.SetDataSource(folhaPagamentoDataSet);

                        report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Path.Combine(caminhoPasta, $"{folha.Funcionario.Nome}.pdf"));

                        progressoAtual += incremento;
                    }

                    progressoAtual = 100;
                    progressoValor.Report(progressoAtual);
                    progressoDescricao.Report($@"Folhas De Pagamento Foram Exportadas Em PDF Com Sucesso Em {caminhoPasta}!");
                }
                catch (Exception ex)
                {
                    MessageBoxService.Show(ex.Message);
                }
            }
        }

        private void AbrirAdicionarObservacao(object obj)
        {
            AdicionarObservacaoFolhaVM viewModel = new AdicionarObservacaoFolhaVM(_session, FolhaPagamento, new MessageBoxService());
            InserirObservacaoFolha view = new InserirObservacaoFolha
            {
                DataContext = viewModel
            };
            view.ShowDialog();
            OnPropertyChanged("TermoPesquisa");
        }

        private void AbrirCalculoAlmoco(object obj)
        {
            CalculoDeBonusMensalPorDiaVM almocoVM = new CalculoDeBonusMensalPorDiaVM(DataEscolhida, new MessageBoxService(), new CalculoDeAlmoco());
            CalculoBonusMensalPorDiaView calculoBonusMensalPorDia = new CalculoBonusMensalPorDiaView()
            {
                DataContext = almocoVM
            };
            calculoBonusMensalPorDia.ShowDialog();
            OnPropertyChanged("TermoPesquisa");
        }

        private void AbrirDadosBancarios(object obj)
        {
            VisualizarDadosBancarios view = new VisualizarDadosBancarios(FolhaPagamento.Funcionario);
            view.Show();
        }

        private void AbrirAdicionarSalarioLiquido(object obj)
        {
            AdicionarSalarioLiquidoVM viewModel = new AdicionarSalarioLiquidoVM(_session, _folhaPagamento, new MessageBoxService());
            AdicionarSalarioLiquido view = new AdicionarSalarioLiquido
            {
                DataContext = viewModel
            };
            view.ShowDialog();
            OnPropertyChanged("TermoPesquisa");
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

                foreach (var parcela in FolhaPagamento.Parcelas)
                {
                    parcela.Paga = true;
                }

                var dao = daoEntidade as DAOFolhaPagamento;
                var result = await dao.FecharFolhaDePagamento(FolhaPagamento);

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
            VisualizarHoraExtraVM visualizarHoraExtraVM = new VisualizarHoraExtraVM(DataEscolhida, new MessageBoxService());
            VisualizarHoraExtra visualizarHoraExtra = new VisualizarHoraExtra
            {
                DataContext = visualizarHoraExtraVM
            };
            visualizarHoraExtra.ShowDialog();
        }

        private void AbrirImprimirFolha(object obj)
        {
            TelaRelatorioFolha telaRelatorioFolha = new TelaRelatorioFolha(_session, FolhaPagamento);
            telaRelatorioFolha.Show();
        }

        private void AbrirCalculoPassagem(object obj)
        {
            CalculoDeBonusMensalPorDiaVM onibusVM = new CalculoDeBonusMensalPorDiaVM(DataEscolhida, new MessageBoxService(), new CalculoDePassagem());
            CalculoBonusMensalPorDiaView calculoBonusMensalPorDia = new CalculoBonusMensalPorDiaView()
            {
                DataContext = onibusVM
            };
            calculoBonusMensalPorDia.ShowDialog();
            OnPropertyChanged("TermoPesquisa");
        }

        private void AbrirHoraExtra(object obj)
        {
            AdicionarHoraExtraVM adicionarHoraExtraVM = new AdicionarHoraExtraVM(_session, FolhaPagamento, new MessageBoxService(), false);
            AdicionarHoraExtra adicionarHoraExtra = new AdicionarHoraExtra()
            {
                DataContext = adicionarHoraExtraVM
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
            MessageBoxService.Show(GetResource.GetString("arquivo_excel_sendo_gerado"));
            IsThreadLocked = true;
            await new Excel<FolhaPagamentoModel>(excelStrategy).Salvar(new List<FolhaPagamentoModel>(FolhaPagamentos));
            IsThreadLocked = false;
            MessageBoxService.Show(GetResource.GetString("exportacao_excel_realizada_com_sucesso"));
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
                OnPropertyChanged("TotalAPagar");
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

        public double TotalAPagar
        {
            get => FolhaPagamentos.Select(s => s.ValorATransferir).Sum();
        }
        public string TituloTelaProgresso
        {
            get => _tituloTelaProgresso;
            set
            {
                _tituloTelaProgresso = value;
                OnPropertyChanged("TituloTelaProgresso");
            }
        }
        public string DescricaoBarraProgresso
        {
            get => _descricaoBarraProgresso;
            set
            {
                _descricaoBarraProgresso = value;
                OnPropertyChanged("DescricaoBarraProgresso");
            }
        }
        public double ValorBarraProgresso
        {
            get => _valorBarraProgresso;
            set
            {
                _valorBarraProgresso = value;
                OnPropertyChanged("ValorBarraProgresso");
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
                    if (funcionario.Deletado)
                        continue;

                    folha = new FolhaPagamentoModel
                    {
                        Mes = DataEscolhida.Month,
                        Ano = DataEscolhida.Year,
                        Funcionario = funcionario
                    };
                }
                else
                {
                    if (!folha.Fechada)
                    {
                        if (folha.ValorDoBonusDeMeta > 0)
                        {
                            var mesFolha = new DateTime(folha.Ano, folha.Mes, 1);
                            Bonus bonus = new Bonus
                            {
                                Funcionario = funcionario,
                                Data = new DateTime(folha.Ano, folha.Mes, DateTime.DaysInMonth(folha.Ano, folha.Mes)),
                                Descricao = $"META MÊS {mesFolha.ToString("MMMM", CultureInfo.GetCultureInfo("pt-BR"))} - BASE DE CÁLCULO {(folha.TotalVendido - folha.MetaDeVenda).ToString("C", CultureInfo.GetCultureInfo("pt-BR"))}",
                                Valor = folha.ValorDoBonusDeMeta,
                                MesReferencia = folha.Mes,
                                AnoReferencia = folha.Ano
                            };

                            folha.Bonus.Add(bonus);
                        }
                    }
                }

                //Lista as parcelas somente se folha já existe
                var parcelas = await daoParcela.ListarPorFuncionarioMesAno(folha.Funcionario, folha.Mes, folha.Ano);
                folha.Parcelas = parcelas;

                //Lista todos os bônus do funcionário (inclusive bônus cancelados)
                folha.Bonus = await daoBonus.ListarPorFuncionario(funcionario, DataEscolhida.Month, DataEscolhida.Year);

                //Adiciona bônus mensais
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
                                BonusMensal = true
                            };

                            folha.Bonus.Add(bonus);
                        }
                    }
                }

                //Depois da checagem acima, removo os bônus cancelados da listagem
                if (folha.Bonus != null)
                    folha.Bonus = folha.Bonus.Where(w => w.BonusCancelado == false).ToList();

                folhas.Add(folha);
            }

            FolhaPagamentos = folhas;
        }

        private async void GetFuncionarios()
        {
            _funcionarios = await daoFuncionario.ListarIncluindoDeletado();
        }
    }
}

using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Filter;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.Reporting.WinForms;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.View.Interfaces;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;
using VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoBonusMeta;
using VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia;
using VandaModaIntimaWpf.ViewModel.FolhaPagamento.Util;
using VandaModaIntimaWpf.ViewModel.Funcionario;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class PesquisarFolhaVM : APesquisarViewModel<Model.FolhaPagamento>
    {
        private DAOFuncionario daoFuncionario;
        private DAOBonus daoBonus;
        private DAOParcela daoParcela;
        private DAODespesa daoDespesa;
        private DAOTipoDespesa daoTipoDespesa;
        private DateTime _dataEscolhida;
        private ObservableCollection<Model.FolhaPagamento> _folhaPagamentos;
        private ObservableCollection<ICalculaBonusMeta> _listaCalculoBonusMeta;
        private Model.FolhaPagamento _folhaPagamento;
        private IList<Model.Funcionario> _funcionarios;
        private double _totalEmPassagem;
        private double _totalEmAlimentacao;
        private double _totalEmMeta;
        private IConfiguraReportViewer configuraReportViewer;
        private ICalculaBonusMeta _calculaBonusMeta;

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
        public ICommand GerarContrachequeDeArquivoDoContadorComando { get; set; }
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
            daoTipoDespesa = new DAOTipoDespesa(_session);
            daoDespesa = new DAODespesa(_session);
            daoParcela = new DAOParcela(_session);
            daoBonus = new DAOBonus(_session);
            pesquisarViewModelStrategy = new PesquisarFolhaMsgVMStrategy();
            excelStrategy = new FolhaPagamentoExcelStrategy();
            FolhaPagamentos = new ObservableCollection<Model.FolhaPagamento>();
            cancellationTokenSource = new CancellationTokenSource();
            configuraReportViewer = new ConfiguraReportViewerFolha(_session);

            GetListaCalculoBonusMeta();
            GetFuncionarios();

            CalculaBonusMeta = ListaCalculoBonusMeta.Where(w => w.GetType().Name.Equals(Config.RetornaMetodoCalculoDeBonusDeMeta().GetType().Name)).FirstOrDefault();

            PropertyChanged += PesquisarFolhaVM_PropertyChanged;

            DataEscolhida = DateTime.Now;

            try
            {
                if (DateTime.Now.Day <= DateTimeUtil.RetornaDiaUtilComFeriado(5, DataEscolhida.Month, DataEscolhida.Year).Day)
                {
                    DataEscolhida = DateTime.Now.AddMonths(-1);
                }
            }
            catch (FileNotFoundException fex)
            {
                _messageBoxService.Show(fex.Message);
                if (DateTime.Now.Day <= DateTimeUtil.RetornaDiaUtilSemFeriado(5, DataEscolhida.Month, DataEscolhida.Year).Day)
                {
                    DataEscolhida = DateTime.Now.AddMonths(-1);
                }
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
            GerarContrachequeDeArquivoDoContadorComando = new RelayCommand(GerarContrachequeDeArquivoDoContador);
        }

        /// <summary>
        /// Este método serve para dividir o arquivo de contracheque enviado por walcler no meio horizontalmente, assim fazendo um arquivo pdf de cada contracheque de cada funcionário
        /// </summary>
        /// <param name="obj"></param>
        private async void GerarContrachequeDeArquivoDoContador(object obj)
        {
            try
            {
                if (obj == null)
                    throw new Exception($"Parâmetro de comando não configurado em Pesquisar Folha De Pagamento.");

                var fileBrowserDialog = obj as IOpenFileDialog;
                string caminho = fileBrowserDialog.OpenFileDialog();

                if (caminho != null)
                {
                    string caminhoPasta = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(caminho), "CONTRACHEQUES DESMEMBRADOS");
                    Directory.CreateDirectory(caminhoPasta);

                    using (XPdfForm documento = XPdfForm.FromFile(caminho))
                    {
                        int numPaginas = documento.PageCount;

                        CancellationToken token = cancellationTokenSource.Token;
                        VisibilidadeStatusBar = Visibility.Visible;

                        //Se der exceção ao executar no debug, aperte em Continue que flui normalmente
                        Task task = Task.Run(() =>
                        {
                            token.ThrowIfCancellationRequested();

                            setIsIndefinidaBarraProgresso.Report(true);
                            setDescricaoBarraProgresso.Report("Iniciando Separação e Exportação De Contracheques Para PDF");
                            double incremento = 100.0 / numPaginas;
                            setValorBarraProgresso.Report(-1);
                            setIsIndefinidaBarraProgresso.Report(false);

                            //Loop para cada página do pdf
                            for (int pagIndex = 0; pagIndex < numPaginas; pagIndex++)
                            {
                                token.ThrowIfCancellationRequested();
                                setDescricaoBarraProgresso.Report($"Processando Página {pagIndex + 1}");

                                documento.PageNumber = pagIndex + 1; //Configuro qual página do documento estou manipulando
                                PdfSharp.Pdf.PdfDocument TopOutput = new PdfSharp.Pdf.PdfDocument(); //Representa o documento PDF resultante da metade superior da página do PDF
                                PdfSharp.Pdf.PdfDocument BottomOutput = new PdfSharp.Pdf.PdfDocument(); //Representa o documento PDF resultante da metade inferior da página do PDF

                                //Obtenho as dimensões da página atual
                                double larguraPagina = documento.PointWidth;
                                double alturaPagina = documento.PointHeight;
                                double meiaAlturaPagina = (alturaPagina / 2) - 15; //Ponto onde irei dividir a página ao meio

                                //Obtendo metade superior e desenhando em novo arquivo PDF
                                PdfSharp.Pdf.PdfPage topPage = TopOutput.AddPage();
                                topPage.Width = XUnit.FromPoint(larguraPagina); //Determino a largura da página resultante
                                topPage.Height = XUnit.FromPoint(meiaAlturaPagina); //Determino a altura da página resultante

                                using (XGraphics gfxTop = XGraphics.FromPdfPage(topPage))
                                {
                                    gfxTop.Save();

                                    // Source rect captures the top half; Destination rect paints it exactly onto the new page
                                    XRect srcRectTop = new XRect(0, 0, larguraPagina, meiaAlturaPagina);
                                    XRect destRectTop = new XRect(0, 0, larguraPagina, alturaPagina);

                                    gfxTop.IntersectClip(srcRectTop);
                                    gfxTop.DrawImage(documento, destRectTop, srcRectTop, XGraphicsUnit.Point);
                                    gfxTop.Restore();
                                }

                                //Obtendo metade inferior e desenhando em novo arquivo PDF
                                PdfSharp.Pdf.PdfPage bottomPage = BottomOutput.AddPage();
                                bottomPage.Width = XUnit.FromPoint(larguraPagina);
                                bottomPage.Height = XUnit.FromPoint(meiaAlturaPagina);

                                using (XGraphics gfxBottom = XGraphics.FromPdfPage(bottomPage))
                                {
                                    gfxBottom.Save();
                                    gfxBottom.TranslateTransform(0, -meiaAlturaPagina);

                                    // Source rect shifts right by half-width; Destination paints starting at 0 on the new canvas
                                    XRect srcRectBottom = new XRect(0, meiaAlturaPagina, larguraPagina, meiaAlturaPagina);
                                    XRect destRectBottom = new XRect(0, 0, larguraPagina, alturaPagina);

                                    gfxBottom.DrawImage(documento, destRectBottom, srcRectBottom, XGraphicsUnit.Point);

                                    gfxBottom.Restore();
                                }

                                var topOutputCaminho = System.IO.Path.Combine(caminhoPasta, $"pagina-{pagIndex + 1}-1.pdf");
                                var bottomOutputCaminho = System.IO.Path.Combine(caminhoPasta, $"pagina-{pagIndex + 1}-2.pdf");

                                TopOutput.Save(topOutputCaminho);
                                BottomOutput.Save(bottomOutputCaminho);

                                topOutputCaminho = ExtraiNomeERenomeia(caminhoPasta, topOutputCaminho);
                                bottomOutputCaminho = ExtraiNomeERenomeia(caminhoPasta, bottomOutputCaminho);

                                ConfiguraSenhaContraCheque(topOutputCaminho);
                                ConfiguraSenhaContraCheque(bottomOutputCaminho);

                                setValorBarraProgresso.Report(incremento);
                            }

                            setDescricaoBarraProgresso.Report($"Contracheques foram desmembrados com sucesso.");
                        }, token);

                        try
                        {
                            await task;
                        }
                        catch (OperationCanceledException)
                        {
                            _messageBoxService.Show("Desmembramento de contracheques foi cancelado pela usuário");
                        }
                        finally
                        {
                            cancellationTokenSource.Dispose();
                            cancellationTokenSource = new CancellationTokenSource();
                        }

                        await task.ContinueWith(t =>
                        {
                            VisibilidadeStatusBar = Visibility.Collapsed;
                            if (!task.IsCanceled)
                                _messageBoxService.Show("Contracheques foram desmembrados com sucesso e salvos junto com arquivo original na pasta CONTRACHEQUES DESMEMBRADOS.");
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _messageBoxService.Show(ex.Message);
            }
        }

        private async void ConfiguraSenhaContraCheque(string caminhoPdf)
        {
            PdfSharp.Pdf.PdfDocument arquivoPdf = PdfSharp.Pdf.IO.PdfReader.Open(caminhoPdf, PdfDocumentOpenMode.Modify);
            PdfSecuritySettings securitySettings = arquivoPdf.SecuritySettings;

            var funcionario = await daoFuncionario.GetPorNome(System.IO.Path.GetFileNameWithoutExtension(caminhoPdf));

            securitySettings.UserPassword = funcionario.Cpf.Substring(0, 4);

            // 4. (Optional) Restrict user capabilities
            securitySettings.PermitModifyDocument = false;
            securitySettings.PermitExtractContent = false;

            // 5. Save the encrypted document
            arquivoPdf.Save(caminhoPdf);
            arquivoPdf.Close();
        }

        /// <summary>
        /// Extrai nome funcionário de área pré-determinada do contracheque da Real Contabilidade.
        /// </summary>
        /// <param name="caminhoPasta">Diretório onde estão e serão salvos os contracheques desmembrados com novos nomes</param>
        /// <param name="outputCaminho">Caminho do arquivo de contracheque desmembrado</param>
        /// <returns>Retorna caminho do arquivo com novo nome</returns>
        private static string ExtraiNomeERenomeia(string caminhoPasta, string outputCaminho)
        {
            //Área onde geralmente está o nome no contracheque (pode mudar caso layout do contracheque seja alterado pela contabilidade)
            //Esta área somente estará correta se o pdf estiver em landscape. Não mudar para retrato.
            Rectangle RetanguloNome = new Rectangle(78, 326, 275, 20);

            using (iText.Kernel.Pdf.PdfReader reader = new iText.Kernel.Pdf.PdfReader(outputCaminho))
            {
                using (iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(reader))
                {
                    var page = pdfDoc.GetPage(1);

                    // 2. Set up the region filter
                    TextRegionEventFilter regionFilter = new TextRegionEventFilter(RetanguloNome);
                    var strategy = new FilteredTextEventListener(new LocationTextExtractionStrategy(), regionFilter);

                    // 3. Extract the text bounded by the rectangle
                    string extractedText = PdfTextExtractor.GetTextFromPage(page, strategy).Trim();

                    reader.Close();

                    string caminho = System.IO.Path.Combine(caminhoPasta, extractedText + ".pdf");

                    File.Move(outputCaminho, caminho, true);

                    return caminho;
                }
            }
        }

        private void PesquisarFolhaVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "DataEscolhida":
                    OnPropertyChanged("TermoPesquisa");
                    break;
                case "CalculaBonusMeta":
                    Config.SetMetodoCalculoDeBonusDeMeta(CalculaBonusMeta);
                    OnPropertyChanged("TermoPesquisa");
                    break;
            }
        }

        /// <summary>
        /// Comando responsável por adicionar os valores pagos aos funcionários como despesas no sistema.
        /// </summary>
        /// <param name="obj"></param>
        private async void AdicionarValoresDespesa(object obj)
        {
            //Agrupamento é feito por loja em que funcionário está trabalhando atualmente.
            IList<Model.Despesa> despesas = new List<Model.Despesa>();
            var folhasPorLojaSoma = FolhaPagamentos.GroupBy(g => g.Funcionario.LojaTrabalho)
                .Select(g => new { Loja = g.Key, Soma = g.Sum(s => s.ValorATransferir), Vencimento = g.First().Vencimento });

            foreach (var item in folhasPorLojaSoma)
            {
                var despesa = new Model.Despesa
                {
                    TipoDespesa = await daoTipoDespesa.RetornaTipoDespesaEmpresarial(),
                    Loja = item.Loja,
                    Valor = item.Soma,
                    DataVencimento = item.Vencimento,
                    Data = item.Vencimento,
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
                IList<Tuple<string, byte[]>> listaBytes = new List<Tuple<string, byte[]>>();

                if (caminhoPasta != null)
                {
                    CancellationToken token = cancellationTokenSource.Token;
                    VisibilidadeStatusBar = Visibility.Visible;

                    //Se der exceção ao executar no debug, aperte em Continue que flui normalmente
                    Task task = Task.Run(() =>
                    {
                        token.ThrowIfCancellationRequested();

                        setIsIndefinidaBarraProgresso.Report(true);
                        setDescricaoBarraProgresso.Report("Iniciando Exportação De Folhas De Pagamento Para PDF");
                        double incremento = 100.0 / FolhaPagamentos.Count;
                        setValorBarraProgresso.Report(-1);
                        setIsIndefinidaBarraProgresso.Report(false);

                        foreach (var folha in FolhaPagamentos)
                        {
                            token.ThrowIfCancellationRequested();
                            setDescricaoBarraProgresso.Report($"Gerando folha de {folha.Funcionario.Nome}");

                            var reportDataSource = configuraReportViewer.ConfigurarReportDataSource(folha).Result;
                            var relatorio = new ReportViewer();
                            configuraReportViewer.Configurar(relatorio, reportDataSource, "VandaModaIntimaWpf.View.FolhaPagamento.Relatorios.RelatorioFolhaPagamento.rdlc");
                            byte[] Bytes = relatorio.LocalReport.Render("PDF", "");
                            string caminhoCompleto = System.IO.Path.Combine(caminhoPasta, $"{folha.Funcionario.Nome}.pdf");
                            listaBytes.Add(new Tuple<string, byte[]>(caminhoCompleto, Bytes));
                            setValorBarraProgresso.Report(incremento);
                        }

                        foreach (var tupla in listaBytes)
                        {
                            using (FileStream stream = new FileStream(tupla.Item1, FileMode.Create))
                            {
                                stream.Write(tupla.Item2, 0, tupla.Item2.Length);
                            }
                        }

                        setDescricaoBarraProgresso.Report($"Folhas de pagamento foram exportadas em PDF com sucesso em {caminhoPasta}");
                    }, token);

                    try
                    {
                        await task;
                    }
                    catch (OperationCanceledException)
                    {
                        _messageBoxService.Show("Exportação para PDF foi cancelada pela usuário");
                    }
                    finally
                    {
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
                    parcela.PagaEm = folhasAbertas[0].Vencimento;
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
                    parcela.PagaEm = FolhaPagamento.Vencimento;
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

        public ObservableCollection<ICalculaBonusMeta> ListaCalculoBonusMeta
        {
            get
            {
                return _listaCalculoBonusMeta;
            }

            set
            {
                _listaCalculoBonusMeta = value;
                OnPropertyChanged("ListaCalculoBonusMeta");
            }
        }

        public ICalculaBonusMeta CalculaBonusMeta
        {
            get
            {
                return _calculaBonusMeta;
            }

            set
            {
                _calculaBonusMeta = value;
                OnPropertyChanged("CalculaBonusMeta");
            }
        }

        public override bool Editavel(object parameter)
        {
            return true;
        }

        public override async Task PesquisaItens(string termo)
        {
            var folhas = await PesquisarFolhaPagamentoUtil.GeraListaDeFolhas(Session, _funcionarios, DataEscolhida, CalculaBonusMeta);

            TotalEmPassagem = await daoBonus.SomaPassagemPorMesAno(DataEscolhida);
            TotalEmAlimentacao = await daoBonus.SomaAlimentacaoPorMesAno(DataEscolhida);
            //Bonus de meta não são salvos até que a folha seja fechada então uso linq com a coleção atual para conseguir o valor e não do banco de dados
            TotalEmMeta = folhas.SelectMany(sm => sm.Bonus).Where(w => w.Descricao.Contains("META") || w.Descricao.Contains("COMISSÃO") || w.Descricao.Contains("GRATIFICAÇÃO")).Sum(s => s.Valor);

            FolhaPagamentos = new ObservableCollection<Model.FolhaPagamento>(folhas);
        }

        private async void GetFuncionarios()
        {
            _funcionarios = await daoFuncionario.Listar();
        }

        private void GetListaCalculoBonusMeta()
        {
            ListaCalculoBonusMeta = new ObservableCollection<ICalculaBonusMeta>
            {
                new CalculaBonusMetaMeioPorcento(),
                new CalculaBonusMetaUmPorcento(),
                new CalculaBonusMetaUmPorcentoAposMeta(),
                new CalculaBonusMetaUmPorcentoMais200(),
                new CalculaBonusMetaFinalDeAno(),
                new CalculaBonusMetaUmPorcentoEGratificacao(),
                new CalculoBonusMetaProporcionalMaxUmPorcento()
            };
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

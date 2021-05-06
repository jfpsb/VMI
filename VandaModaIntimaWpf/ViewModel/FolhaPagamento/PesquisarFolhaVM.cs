using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.Util;
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
        private DateTime _dataEscolhida;
        private ObservableCollection<FolhaPagamentoModel> _folhaPagamentos;
        private FolhaPagamentoModel _folhaPagamento;
        private IList<FuncionarioModel> _funcionarios;
        private IFileDialogService _fileDialogService;

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

        public PesquisarFolhaVM(IMessageBoxService messageBoxService, IFileDialogService fileDialogService, IAbrePelaTelaPesquisaService<FolhaPagamentoModel> abrePelaTelaPesquisaService)
            : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            //TODO: excel para folha de pagamento
            daoEntidade = new DAOFolhaPagamento(_session);
            daoFuncionario = new DAOFuncionario(_session);
            daoBonusMensal = new DAOBonusMensal(_session);
            daoBonus = new DAOBonus(_session);
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
        }

        private void ExportarFolhasParaPDF(object obj)
        {
            string caminhoPasta = _fileDialogService.ShowFolderBrowserDialog();

            if (!string.IsNullOrEmpty(caminhoPasta))
            {
                try
                {
                    FolhaPagamentoDataSet folhaPagamentoDataSet = new FolhaPagamentoDataSet();
                    BonusDataSet bonusDataSet = new BonusDataSet();
                    ParcelaDataSet parcelaDataSet = new ParcelaDataSet();

                    foreach (var folha in FolhaPagamentos)
                    {
                        folhaPagamentoDataSet.Clear();
                        bonusDataSet.Clear();
                        parcelaDataSet.Clear();

                        foreach (var bonus in folha.Bonus)
                        {
                            var brow = bonusDataSet.Bonus.NewBonusRow();
                            brow.id = bonus.Id.ToString();
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

                        var fprow = folhaPagamentoDataSet.FolhaPagamento.NewFolhaPagamentoRow();
                        fprow.mes = folha.Mes.ToString();
                        fprow.ano = folha.Ano.ToString();
                        fprow.mesreferencia = folha.MesReferencia;
                        fprow.vencimento = folha.Vencimento.ToString("dd/MM/yyyy");
                        fprow.funcionario = folha.Funcionario.Nome;
                        fprow.valor_a_transferir = folha.ValorATransferir.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                        fprow.salario_liquido = folha.SalarioLiquido.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                        fprow.observacao = folha.Observacao;

                        folhaPagamentoDataSet.FolhaPagamento.AddFolhaPagamentoRow(fprow);

                        var report = new RelatorioFolhaPagamento();
                        report.Load("/View/FolhaPagamento/Relatorios/RelatorioFolhaPagamento.rpt");
                        report.Subreports[0].SetDataSource(bonusDataSet);
                        report.Subreports[1].SetDataSource(parcelaDataSet);
                        report.SetDataSource(folhaPagamentoDataSet);

                        //var stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                        report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Path.Combine(caminhoPasta, $"{folha.Funcionario.Nome}.pdf"));
                    }

                    MessageBoxService.Show($@"Folhas De Pagamento Foram Exportadas Em PDF Com Sucesso Em {caminhoPasta}!", pesquisarViewModelStrategy.PesquisarEntidadeCaption());
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
            CalculoBonusMensalPorDia calculoBonusMensalPorDia = new CalculoBonusMensalPorDia()
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
                var bonusMensaisEmFolhas = folhasAbertas.Select(s => s.Bonus).SelectMany(s => s.Where(w => w.BonusMensal)).ToList();

                var bonusResult = await daoBonus.InserirOuAtualizar(bonusMensaisEmFolhas);

                if (!bonusResult)
                    MessageBoxService.Show("Erro Ao Salvar Bônus Mensais. As Folhas Não Poderão Ser Fechadas. Tente Novamente Ou Contate O Suporte.");

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
            VisualizarHoraExtraVM visualizarHoraExtraVM = new VisualizarHoraExtraVM(DataEscolhida, new MessageBoxService());
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

        private void AbrirCalculoPassagem(object obj)
        {
            CalculoDeBonusMensalPorDiaVM onibusVM = new CalculoDeBonusMensalPorDiaVM(DataEscolhida, new MessageBoxService(), new CalculoDePassagem());
            CalculoBonusMensalPorDia calculoBonusMensalPorDia = new CalculoBonusMensalPorDia()
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
                        Funcionario = funcionario
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
                            BonusMensal = true
                        };

                        folha.Bonus.Add(bonus);
                    }
                }

                //Depois da checagem acima, removo os bônus cancelados da listagem
                folha.Bonus = folha.Bonus.Where(w => w.BonusCancelado == false).ToList();

                folhas.Add(folha);
            }

            FolhaPagamentos = folhas;
        }

        private async void GetFuncionarios()
        {
            _funcionarios = await daoFuncionario.Listar<FuncionarioModel>();
        }
    }
}

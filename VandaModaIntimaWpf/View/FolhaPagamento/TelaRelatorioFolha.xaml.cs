using System.Globalization;
using System.Windows;
using VandaModaIntimaWpf.View.FolhaPagamento.Relatorios;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    /// <summary>
    /// Interaction logic for TelaRelatorioFolha.xaml
    /// </summary>
    public partial class TelaRelatorioFolha : Window
    {
        public TelaRelatorioFolha()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
        }

        public TelaRelatorioFolha(Model.FolhaPagamento FolhaPagamento)
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();

            FolhaPagamentoDataSet folhaPagamentoDataSet = new FolhaPagamentoDataSet();
            BonusDataSet bonusDataSet = new BonusDataSet();
            ParcelaDataSet parcelaDataSet = new ParcelaDataSet();

            foreach (var bonus in FolhaPagamento.Bonus)
            {
                var brow = bonusDataSet.Bonus.NewBonusRow();
                brow.id = bonus.Id.ToString();
                brow.data = bonus.DataString;
                brow.descricao = bonus.Descricao;
                brow.valor = bonus.Valor.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                brow.total_bonus = FolhaPagamento.TotalBonus.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));

                bonusDataSet.Bonus.AddBonusRow(brow);
            }

            foreach (var parcela in FolhaPagamento.Parcelas)
            {
                var prow = parcelaDataSet.Parcela.NewParcelaRow();
                prow.id = parcela.Id.ToString();
                prow.numero = parcela.Numero.ToString();
                prow.valor = parcela.Valor.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                prow.data_adiantamento = parcela.Adiantamento.DataString;
                prow.numero_com_total = parcela.NumeroComTotal;
                prow.total_adiantamentos = FolhaPagamento.TotalAdiantamentos.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                prow.descricao = parcela.Adiantamento.Descricao;

                parcelaDataSet.Parcela.AddParcelaRow(prow);
            }

            var fprow = folhaPagamentoDataSet.FolhaPagamento.NewFolhaPagamentoRow();
            fprow.mes = FolhaPagamento.Mes.ToString();
            fprow.ano = FolhaPagamento.Ano.ToString();
            fprow.mesreferencia = FolhaPagamento.MesReferencia;
            fprow.vencimento = FolhaPagamento.Vencimento.ToString("dd/MM/yyyy");
            fprow.funcionario = FolhaPagamento.Funcionario.Nome;
            fprow.valor_a_transferir = FolhaPagamento.ValorATransferir.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            fprow.salario_liquido = FolhaPagamento.SalarioLiquido.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            fprow.observacao = FolhaPagamento.Observacao;
            fprow.valordameta = FolhaPagamento.MetaDeVenda.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            fprow.totalvendido = FolhaPagamento.TotalVendido.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));

            folhaPagamentoDataSet.FolhaPagamento.AddFolhaPagamentoRow(fprow);

            var report = new RelatorioFolhaPagamento();
            report.Subreports[0].SetDataSource(bonusDataSet);
            report.Subreports[1].SetDataSource(parcelaDataSet);
            report.SetDataSource(folhaPagamentoDataSet);
            FolhaReport.ViewerCore.EnableDrillDown = false;
            FolhaReport.ViewerCore.ReportSource = report;
        }
    }
}

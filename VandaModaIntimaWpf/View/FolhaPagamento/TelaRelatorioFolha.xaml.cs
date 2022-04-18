using NHibernate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.View.FolhaPagamento.Relatorios;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    /// <summary>
    /// Interaction logic for TelaRelatorioFolha.xaml
    /// </summary>
    public partial class TelaRelatorioFolha : Window
    {
        private DAOParcela daoParcela;
        private DAOHoraExtra daoHoraExtra;
        private string caminhoFolhaPagamentoVMI = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Vanda Moda Íntima", "Folha De Pagamento");
        public TelaRelatorioFolha()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
            FolhaReport.Owner = this;
        }

        public TelaRelatorioFolha(ISession session, Model.FolhaPagamento FolhaPagamento)
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
            FolhaReport.Owner = this;

            daoParcela = new DAOParcela(session);
            daoHoraExtra = new DAOHoraExtra(session);
            FolhaPagamentoDataSet folhaPagamentoDataSet = new FolhaPagamentoDataSet();
            BonusDataSet bonusDataSet = new BonusDataSet();
            ParcelaDataSet parcelaDataSet = new ParcelaDataSet();

            var report = new RelatorioFolhaPagamento();
            int i = 0;

            foreach (var bonus in FolhaPagamento.Bonus)
            {
                var brow = bonusDataSet.Bonus.NewBonusRow();
                brow.id = i++.ToString();
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

            var parcelasNaoPagas = GetRestanteAdiantamento(FolhaPagamento.Funcionario, FolhaPagamento);

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
            fprow.restanteadiantamento = parcelasNaoPagas.Result;

            var calendarioPassagem = Path.Combine(caminhoFolhaPagamentoVMI, FolhaPagamento.Funcionario.Nome, FolhaPagamento.Ano.ToString(), FolhaPagamento.Mes.ToString(), "CalendarioPassagem.png");
            var calendarioAlimentacao = Path.Combine(caminhoFolhaPagamentoVMI, FolhaPagamento.Funcionario.Nome, FolhaPagamento.Ano.ToString(), FolhaPagamento.Mes.ToString(), "CalendarioAlimentacao.png");

            fprow.calendariopassagem = calendarioPassagem;
            fprow.calendarioalimentacao = calendarioAlimentacao;

            report.ReportDefinition.ReportObjects["TxtCalendarioPassagens"].ObjectFormat.EnableSuppress = !File.Exists(calendarioPassagem);
            report.ReportDefinition.ReportObjects["PicPassagens"].ObjectFormat.EnableSuppress = !File.Exists(calendarioPassagem);

            report.ReportDefinition.ReportObjects["TxtCalendarioAlimentacao"].ObjectFormat.EnableSuppress = !File.Exists(calendarioAlimentacao);
            report.ReportDefinition.ReportObjects["PicAlimentacao"].ObjectFormat.EnableSuppress = !File.Exists(calendarioAlimentacao);

            //Se não existe nenhum dos dois calendários salvos esconde a sessão
            report.DetailSection3.SectionFormat.EnableSuppress = !(File.Exists(calendarioPassagem) || File.Exists(calendarioAlimentacao));

            fprow.horaextra100 = "00:00";
            fprow.horaextra55 = "00:00";

            var horasExtras = daoHoraExtra.ListarPorAnoMesFuncionario(FolhaPagamento.Ano, FolhaPagamento.Mes, FolhaPagamento.Funcionario).Result;
            var he100 = horasExtras.Where(w => w.TipoHoraExtra.Id == 1).SingleOrDefault();
            var henormal = horasExtras.Where(w => w.TipoHoraExtra.Id == 2).SingleOrDefault();

            if (he100 != null)
            {
                fprow.horaextra100 = he100.EmTimeSpan.ToString("hh\\:mm");
            }

            if (henormal != null)
            {
                fprow.horaextra55 = henormal.EmTimeSpan.ToString("hh\\:mm");
            }

            folhaPagamentoDataSet.FolhaPagamento.AddFolhaPagamentoRow(fprow);

            report.Subreports[0].SetDataSource(bonusDataSet);
            report.Subreports[1].SetDataSource(parcelaDataSet);
            report.SetDataSource(folhaPagamentoDataSet);
            FolhaReport.ViewerCore.EnableDrillDown = false;
            FolhaReport.ViewerCore.ReportSource = report;
        }

        private async Task<string> GetRestanteAdiantamento(Model.Funcionario funcionario, Model.FolhaPagamento folha)
        {
            var restante = await daoParcela.ListarPorFuncionarioNaoPagasExcetoMesAno(funcionario, folha.Mes, folha.Ano);
            return restante.Sum(s => s.Valor).ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
        }
    }
}

using Microsoft.Reporting.WinForms;
using NHibernate;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.View.Interfaces;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    public class ConfiguraReportViewerFolha : IConfiguraReportViewer
    {
        private ISession _session;
        private DAOParcela daoParcela;
        private DAOHoraExtra daoHoraExtra;
        private DAOFaltas daoFalta;
        private string caminhoFolhaPagamentoVMI = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Vanda Moda Intima", "Folha De Pagamento");
        private Model.FolhaPagamento _folha;
        public ConfiguraReportViewerFolha(ISession session)
        {
            _session = session;
            daoParcela = new DAOParcela(_session);
            daoHoraExtra = new DAOHoraExtra(_session);
            daoFalta = new DAOFaltas(_session);
        }
        public void Configurar(ReportViewer relatorio, ReportDataSource reportDataSource, string embeddedPath)
        {
            ReportViewerUtil.ConfiguraReportViewer(relatorio,
                    embeddedPath,
                    reportDataSource,
                    LocalReport_SubreportProcessing);
        }

        public async Task<ReportDataSource> ConfigurarReportDataSource(params object[] fonte)
        {
            _folha = fonte[0] as Model.FolhaPagamento;
            FolhaPagamentoDataSet folhaPagamentoDataSet = new FolhaPagamentoDataSet();

            var parcelasNaoPagas = GetRestanteAdiantamento(_folha.Funcionario, _folha);

            var fprow = folhaPagamentoDataSet.FolhaPagamento.NewFolhaPagamentoRow();
            fprow.cpf_funcionario = _folha.Funcionario.Cpf;
            fprow.mes = _folha.Mes.ToString();
            fprow.ano = _folha.Ano.ToString();
            fprow.mesreferencia = _folha.MesReferencia;
            fprow.vencimento = _folha.Vencimento.ToString("dd/MM/yyyy");
            fprow.funcionario = _folha.Funcionario.Nome;
            fprow.valor_a_transferir = _folha.ValorATransferir.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            fprow.salario_liquido = _folha.SalarioLiquido.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            fprow.observacao = _folha.Observacao;
            fprow.valordameta = _folha.MetaDeVenda.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            fprow.totalvendido = _folha.TotalVendido.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            fprow.restanteadiantamento = parcelasNaoPagas.Result;

            var calendarioPassagem = Path.Combine(caminhoFolhaPagamentoVMI, _folha.Funcionario.Nome, _folha.Ano.ToString(), _folha.Mes.ToString(), "CalendarioPassagem.png");
            var calendarioAlimentacao = Path.Combine(caminhoFolhaPagamentoVMI, _folha.Funcionario.Nome, _folha.Ano.ToString(), _folha.Mes.ToString(), "CalendarioAlimentacao.png");

            fprow.calendariopassagem = File.Exists(calendarioPassagem) ? "file:\\" + calendarioPassagem : null;
            fprow.calendarioalimentacao = File.Exists(calendarioAlimentacao) ? "file:\\" + calendarioAlimentacao : null;

            fprow.horaextra100 = "00:00";
            fprow.horaextra55 = "00:00";
            fprow.faltas = "00:00";

            var horasExtras = daoHoraExtra.ListarPorMesFuncionario(_folha.Ano, _folha.Mes, _folha.Funcionario).Result;
            var he100 = horasExtras.Where(w => w.TipoHoraExtra.Descricao.Equals("HORA EXTRA C/100%")).SingleOrDefault();
            var henormal = horasExtras.Where(w => w.TipoHoraExtra.Descricao.Equals("HORA EXTRA C/060%")).SingleOrDefault();
            var falta = await daoFalta.ListarFaltasPorMesFuncionarioSoma(_folha.Ano, _folha.Mes, _folha.Funcionario);

            if (he100 != null)
            {
                fprow.horaextra100 = he100.TotalEmString;
            }

            if (henormal != null)
            {
                fprow.horaextra55 = henormal.TotalEmString;
            }

            if (falta != null)
            {
                fprow.faltas = falta.TotalEmString;
            }

            folhaPagamentoDataSet.FolhaPagamento.AddFolhaPagamentoRow(fprow);

            return new ReportDataSource("DataSetFolha", folhaPagamentoDataSet.Tables[0]);
        }

        private async Task<string> GetRestanteAdiantamento(Model.Funcionario funcionario, Model.FolhaPagamento folha)
        {
            var restante = await daoParcela.ListarPorFuncionarioNaoPagasExcetoMesAno(funcionario, folha.Mes, folha.Ano);
            return restante.Sum(s => s.Valor).ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
        }

        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            BonusDataSet bonusDataSet = new BonusDataSet();
            ParcelaDataSet parcelaDataSet = new ParcelaDataSet();

            int i = 0;

            foreach (var bonus in _folha.Bonus)
            {
                var brow = bonusDataSet.Bonus.NewBonusRow();
                brow.id = i++.ToString();
                brow.data = bonus.DataString;
                brow.descricao = bonus.Descricao;
                brow.valor = bonus.Valor.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                brow.total_bonus = _folha.TotalBonus.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));

                bonusDataSet.Bonus.AddBonusRow(brow);
            }

            foreach (var parcela in _folha.Parcelas)
            {
                var prow = parcelaDataSet.Parcela.NewParcelaRow();
                prow.id = parcela.Id.ToString();
                prow.numero = parcela.Numero.ToString();
                //prow.paga = parcela.Paga ? "SIM":"NÃO";
                prow.pagaem = parcela.PagaEm?.ToString("d", CultureInfo.CreateSpecificCulture("pt-BR"));
                prow.valor = parcela.Valor.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                prow.data_adiantamento = parcela.Adiantamento.DataString;
                prow.numero_com_total = parcela.NumeroComTotal;
                prow.total_adiantamentos = _folha.TotalAdiantamentos.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                prow.descricao = parcela.Adiantamento.Descricao;

                parcelaDataSet.Parcela.AddParcelaRow(prow);
            }

            ReportDataSource reportDataSource1 = new ReportDataSource("DataSetBonus", bonusDataSet.Tables[0]);
            ReportDataSource reportDataSource2 = new ReportDataSource("DataSetParcela", parcelaDataSet.Tables[0]);

            e.DataSources.Add(reportDataSource1);
            e.DataSources.Add(reportDataSource2);
        }
    }
}

using NHibernate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.CrystalReports
{
    public class CRFolhaPagamentoParaPDF : ICrystalReportsParaPDF<Model.FolhaPagamento>
    {
        private DAOParcela daoParcela;
        private DAOHoraExtra daoHoraExtra;
        private string caminhoFolhaPagamentoVMI = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Vanda Moda Íntima", "Folha De Pagamento");
        public CRFolhaPagamentoParaPDF(ISession session)
        {
            daoParcela = new DAOParcela(session);
            daoHoraExtra = new DAOHoraExtra(session);
        }
        public Task Salvar(string caminhoPasta,
            IList<Model.FolhaPagamento> lista,
            IProgress<double> valorBarraProgresso,
            IProgress<string> descricaoBarraProgresso,
            IProgress<bool> isIndeterminadaBarraProgresso,
            CancellationToken token)
        {
            //Task task = Task.Run(() =>
            //{
            //    isIndeterminadaBarraProgresso.Report(true);
            //    descricaoBarraProgresso.Report("Iniciando Exportação De Folhas De Pagamento Para PDF");
            //    double incremento = 100.0 / lista.Count;

            //    FolhaPagamentoDataSet folhaPagamentoDataSet = new FolhaPagamentoDataSet();
            //    BonusDataSet bonusDataSet = new BonusDataSet();
            //    ParcelaDataSet parcelaDataSet = new ParcelaDataSet();

            //    //var report = new RelatorioFolhaPagamento();
            //    //report.Load("/View/FolhaPagamento/Relatorios/RelatorioFolhaPagamento.rpt");

            //    valorBarraProgresso.Report(-1);
            //    isIndeterminadaBarraProgresso.Report(false);
            //    foreach (var folha in lista)
            //    {
            //        token.ThrowIfCancellationRequested();

            //        folhaPagamentoDataSet.Clear();
            //        bonusDataSet.Clear();
            //        parcelaDataSet.Clear();

            //        descricaoBarraProgresso.Report($"Gerando Folha de {folha.Funcionario.Nome}");

            //        int i = 0;
            //        foreach (var bonus in folha.Bonus)
            //        {
            //            var brow = bonusDataSet.Bonus.NewBonusRow();

            //            //Os bônus mensais que ainda não foram salvos tem Id 0, se tentar adicionar dois items com mesma Id dá erro.
            //            //Então uso essa variável i apenas para setar uma Id diferente para cada bônus
            //            brow.id = i++.ToString();
            //            brow.data = bonus.DataString;
            //            brow.descricao = bonus.Descricao;
            //            brow.valor = bonus.Valor.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            //            brow.total_bonus = folha.TotalBonus.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));

            //            bonusDataSet.Bonus.AddBonusRow(brow);
            //        }

            //        foreach (var parcela in folha.Parcelas)
            //        {
            //            var prow = parcelaDataSet.Parcela.NewParcelaRow();
            //            prow.id = parcela.Id.ToString();
            //            prow.numero = parcela.Numero.ToString();
            //            prow.valor = parcela.Valor.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            //            prow.data_adiantamento = parcela.Adiantamento.DataString;
            //            prow.numero_com_total = parcela.NumeroComTotal;
            //            prow.total_adiantamentos = folha.TotalAdiantamentos.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            //            prow.descricao = parcela.Adiantamento.Descricao;

            //            parcelaDataSet.Parcela.AddParcelaRow(prow);
            //        }

            //        var parcelasNaoPagas = daoParcela.ListarPorFuncionarioNaoPagas(folha.Funcionario).Result;

            //        var fprow = folhaPagamentoDataSet.FolhaPagamento.NewFolhaPagamentoRow();
            //        fprow.mes = folha.Mes.ToString();
            //        fprow.ano = folha.Ano.ToString();
            //        fprow.mesreferencia = folha.MesReferencia;
            //        fprow.vencimento = folha.Vencimento.ToString("dd/MM/yyyy");
            //        fprow.funcionario = folha.Funcionario.Nome;
            //        fprow.valor_a_transferir = folha.ValorATransferir.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            //        fprow.salario_liquido = folha.SalarioLiquido.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            //        fprow.observacao = folha.Observacao;
            //        fprow.valordameta = folha.MetaDeVenda.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            //        fprow.totalvendido = folha.TotalVendido.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            //        fprow.restanteadiantamento = parcelasNaoPagas.Sum(s => s.Valor).ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));

            //        var calendarioPassagem = Path.Combine(caminhoFolhaPagamentoVMI, folha.Funcionario.Nome, folha.Ano.ToString(), folha.Mes.ToString(), "CalendarioPassagem.png");
            //        var calendarioAlimentacao = Path.Combine(caminhoFolhaPagamentoVMI, folha.Funcionario.Nome, folha.Ano.ToString(), folha.Mes.ToString(), "CalendarioAlimentacao.png");

            //        fprow.calendariopassagem = calendarioPassagem;
            //        fprow.calendarioalimentacao = calendarioAlimentacao;

            //        report.ReportDefinition.ReportObjects["TxtCalendarioPassagens"].ObjectFormat.EnableSuppress = !File.Exists(calendarioPassagem);
            //        report.ReportDefinition.ReportObjects["PicPassagens"].ObjectFormat.EnableSuppress = !File.Exists(calendarioPassagem);

            //        report.ReportDefinition.ReportObjects["TxtCalendarioAlimentacao"].ObjectFormat.EnableSuppress = !File.Exists(calendarioAlimentacao);
            //        report.ReportDefinition.ReportObjects["PicAlimentacao"].ObjectFormat.EnableSuppress = !File.Exists(calendarioAlimentacao);

            //        //Se não existe nenhum dos dois calendários salvos esconde a sessão
            //        report.DetailSection3.SectionFormat.EnableSuppress = !(File.Exists(calendarioPassagem) || File.Exists(calendarioAlimentacao));

            //        fprow.horaextra100 = "00:00";
            //        fprow.horaextra55 = "00:00";

            //        var horasExtras = daoHoraExtra.ListarPorAnoMesFuncionario(folha.Ano, folha.Mes, folha.Funcionario).Result;
            //        var he100 = horasExtras.Where(w => w.TipoHoraExtra.Descricao.Equals("HORA EXTRA C/100%")).SingleOrDefault();
            //        var henormal = horasExtras.Where(w => w.TipoHoraExtra.Descricao.Equals("HORA EXTRA C/060%")).SingleOrDefault();

            //        if (he100 != null)
            //        {
            //            fprow.horaextra100 = he100.TotalEmString;
            //        }

            //        if (henormal != null)
            //        {
            //            fprow.horaextra55 = henormal.TotalEmString;
            //        }

            //        folhaPagamentoDataSet.FolhaPagamento.AddFolhaPagamentoRow(fprow);

            //        report.Subreports[0].SetDataSource(bonusDataSet);
            //        report.Subreports[1].SetDataSource(parcelaDataSet);
            //        report.SetDataSource(folhaPagamentoDataSet);

            //        report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Path.Combine(caminhoPasta, $"{folha.Funcionario.Nome}.pdf"));
            //        valorBarraProgresso.Report(incremento);
            //    }

            //    descricaoBarraProgresso.Report($"Folhas De Pagamento Foram Exportadas Em PDF Com Sucesso Em {caminhoPasta}");
            //}, token);
            return null;
        }
    }
}

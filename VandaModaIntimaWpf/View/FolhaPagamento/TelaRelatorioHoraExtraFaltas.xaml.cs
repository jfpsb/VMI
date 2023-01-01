using Microsoft.Reporting.WinForms;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    /// <summary>
    /// Interaction logic for TelaRelatorioHoraExtra.xaml
    /// </summary>
    public partial class TelaRelatorioHoraExtraFaltas : Window
    {
        private IList<Model.FolhaPagamento> _folhas;
        private DAOHoraExtra daoHoraExtra;
        private DAOFaltas daoFaltas;
        private DateTime dataEscolhida;
        public TelaRelatorioHoraExtraFaltas()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
        }

        public TelaRelatorioHoraExtraFaltas(ISession session, IList<Model.FolhaPagamento> folhas, DateTime data)
        {
            InitializeComponent();
            _folhas = folhas;
            daoHoraExtra = new DAOHoraExtra(session);
            daoFaltas = new DAOFaltas(session);
            dataEscolhida = data;
        }

        private async void HoraExtraFaltaReportViewer_Load(object sender, EventArgs e)
        {
            HoraExtraFaltasDataSet horaExtraFaltasDataSet = new HoraExtraFaltasDataSet();

            var listaOrdenadaPorLoja = _folhas.OrderBy(o => o.Funcionario.Loja.Cnpj);

            foreach (var folha in listaOrdenadaPorLoja)
            {
                var falta = await daoFaltas.ListarFaltasPorMesFuncionarioSoma(dataEscolhida.Year, dataEscolhida.Month, folha.Funcionario);
                var possuiHorasExtras = (await daoHoraExtra.ListarPorMesFuncionario(dataEscolhida.Year, dataEscolhida.Month, folha.Funcionario)).Any();

                //Exclui do relatório funcionários que não possuem bônus que são pagos em folha, não possuem faltas, nem horas extras.
                if (!folha.Bonus.Any(a => a.PagoEmFolha) && falta == null && !possuiHorasExtras)
                {
                    continue;
                }

                if (falta == null) falta = new Model.Faltas();

                var herow = horaExtraFaltasDataSet.HoraExtra.NewHoraExtraRow();

                herow.cpf_funcionario = folha.Funcionario.Cpf;
                herow.nome_funcionario = folha.Funcionario.Nome;
                herow.nome_loja = folha.Funcionario.Loja.Nome;
                herow.mes_referencia = folha.MesReferencia;

                herow.faltas = falta.TotalEmString;

                horaExtraFaltasDataSet.HoraExtra.AddHoraExtraRow(herow);
            }

            ReportDataSource reportDataSource = new ReportDataSource("DataSetHoraExtraFalta", horaExtraFaltasDataSet.Tables[0]);

            ReportViewerUtil.ConfiguraReportViewer(HoraExtraFaltaReportViewer,
                "VandaModaIntimaWpf.View.FolhaPagamento.Relatorios.RelatorioHoraExtraFaltas.rdlc",
                reportDataSource, SubReportProcessing);

            HoraExtraFaltaReportViewer.LocalReport.Refresh();
            HoraExtraFaltaReportViewer.RefreshReport();
        }

        private async void SubReportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            var cpf = e.Parameters["CPFFuncionario"].Values[0].ToString();
            var folha = _folhas.Where(w => w.Funcionario.Cpf.Equals(cpf)).FirstOrDefault();

            BonusDataSet bonusDataSet = new BonusDataSet();
            HoraExtraSubReportDataSet horaExtraSubReportDataSet = new HoraExtraSubReportDataSet();

            if(folha.Funcionario.Funcao.Nome.Equals("VENDEDOR"))
            {

            }

            int i = 0;

            var Bonus = folha.Bonus;

            Model.Bonus bonusAgregado = new Model.Bonus();
            double soma = Bonus.Where(w => w.PagoEmFolha && w.Descricao.StartsWith("COMISSÃO") || w.Descricao.StartsWith("ADICIONAL")).Sum(s => s.Valor);

            var b = Bonus.Where(w => w.Descricao.StartsWith("COMISSÃO") || w.Descricao.StartsWith("ADICIONAL")).FirstOrDefault();

            foreach (var bonus in Bonus.Where(w => w.PagoEmFolha))
            {
                if (bonus.Descricao.StartsWith("COMISSÃO") || bonus.Descricao.StartsWith("ADICIONAL"))
                {
                    continue;
                }
                var brow = bonusDataSet.Bonus.NewBonusRow();
                brow.id = i++.ToString();
                brow.data = bonus.DataString;
                brow.descricao = bonus.Descricao.Replace(" (PAGO EM FOLHA)", "");
                brow.valor = bonus.Valor.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));

                bonusDataSet.Bonus.AddBonusRow(brow);
            }

            if (soma != 0.0)
            {
                bonusAgregado.Data = b.Data;
                bonusAgregado.Descricao = "COMISSÃO";
                bonusAgregado.Valor = soma;

                var brow2 = bonusDataSet.Bonus.NewBonusRow();
                brow2.id = i++.ToString();
                brow2.data = bonusAgregado.DataString;
                brow2.descricao = bonusAgregado.Descricao;
                brow2.valor = bonusAgregado.Valor.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));

                bonusDataSet.Bonus.AddBonusRow(brow2);
            }

            IList<Model.HoraExtra> horasExtras = await daoHoraExtra.ListarPorMesFuncionarioGroupByTipoHoraExtra(dataEscolhida, folha.Funcionario);

            foreach (var horaextra in horasExtras.OrderBy(o => o.TipoHoraExtra.Descricao))
            {
                var herow = horaExtraSubReportDataSet.horaextra.NewhoraextraRow();
                herow.totalhoras = horaextra.TotalEmString;
                herow.tipohoraextra = horaextra.TipoHoraExtra.Descricao;
                horaExtraSubReportDataSet.horaextra.AddhoraextraRow(herow);
            }

            ReportDataSource reportDataSource1 = new ReportDataSource("DataSetBonus", bonusDataSet.Tables[0]);
            ReportDataSource reportDataSource2 = new ReportDataSource("DataSetHoraExtra", horaExtraSubReportDataSet.Tables[0]);
            e.DataSources.Add(reportDataSource1);
            e.DataSources.Add(reportDataSource2);
        }
    }
}

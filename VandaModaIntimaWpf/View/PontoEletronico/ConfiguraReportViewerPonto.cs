using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Reporting.WinForms;
using NHibernate;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.View.Interfaces;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.View.PontoEletronico
{
    public class ConfiguraReportViewerPonto : IConfiguraReportViewer
    {
        private ISession _session;
        private DAOPontoEletronico daoPonto;
        private Model.Funcionario _funcionario;
        private DateTime _data;
        public ConfiguraReportViewerPonto(ISession session)
        {
            _session = session;
            daoPonto = new DAOPontoEletronico(_session);
        }
        public void Configurar(ReportViewer relatorio, ReportDataSource reportDataSource, string embeddedPath)
        {
            ReportViewerUtil.ConfiguraReportViewer(relatorio,
                    embeddedPath,
                    reportDataSource,
                    null);

            ReportParameterCollection parameters = new ReportParameterCollection();
            parameters.Add(new ReportParameter("NomeFuncionario", _funcionario.Nome));
            parameters.Add(new ReportParameter("MesReferencia", _data.ToString("MMM/yyyy").ToUpper()));

            relatorio.LocalReport.SetParameters(parameters);
            relatorio.LocalReport.Refresh();
            relatorio.RefreshReport();            
        }

        public async Task<ReportDataSource> ConfigurarReportDataSource(params object[] fonte)
        {
            Model.Funcionario funcionario = _funcionario = fonte[0] as Model.Funcionario;
            DateTime data = _data = (DateTime)fonte[1];
            PontoEletronicoDataSet dataSet = new PontoEletronicoDataSet();

            var dias = DateTimeUtil.RetornaDiasEmMes(data.Year, data.Month);

            foreach (var dia in dias)
            {
                var ponto = await daoPonto.ListarPorDiaFuncionario(dia, funcionario);
                var row = dataSet.pontoeletronico.NewpontoeletronicoRow();

                if (ponto == null)
                {
                    ponto = new Model.PontoEletronico
                    {
                        Funcionario = funcionario,
                        Dia = dia
                    };

                    row.entrada = "-- : --";
                    row.saida = "-- : --";
                }
                else
                {
                    row.entrada = ponto.Entrada?.ToString("HH:mm");
                    row.saida = ponto.Saida?.ToString("HH:mm");
                }

                row.dia = ponto.Dia.ToString("dd/MM/yyyy");
                row.intervalos = ponto.IntervalosEmString;
                row.nomefuncionario = ponto.Funcionario.Nome;
                row.mesreferencia = data.ToString("MMM/yyyy");

                dataSet.pontoeletronico.AddpontoeletronicoRow(row);
            }
            return new ReportDataSource("PontoEletronicoDataSet", dataSet.Tables[0]);
        }
    }
}

using Microsoft.Reporting.WinForms;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    /// <summary>
    /// Interaction logic for TelaRelatorioHoraExtra.xaml
    /// </summary>
    public partial class TelaRelatorioHoraExtraFaltas : Window
    {
        private ObservableCollection<Tuple<Model.Funcionario, Model.HoraExtra, Model.HoraExtra, Model.Faltas, DateTime>> _listaHoraExtra;
        public TelaRelatorioHoraExtraFaltas()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
        }

        public TelaRelatorioHoraExtraFaltas(ObservableCollection<Tuple<Model.Funcionario, Model.HoraExtra, Model.HoraExtra, Model.Faltas, DateTime>> listaHoraExtra)
        {
            InitializeComponent();
            _listaHoraExtra = listaHoraExtra;
        }

        private void HoraExtraFaltaReportViewer_Load(object sender, EventArgs e)
        {
            HoraExtraFaltasDataSet horaExtraDataSet = new HoraExtraFaltasDataSet();

            var listaOrdenadaPorLoja = _listaHoraExtra.OrderBy(o => o.Item1.Loja.Cnpj);

            foreach (var horaExtra in listaOrdenadaPorLoja)
            {
                if (!horaExtra.Item2.TotalEmString.Equals("--:--") || !horaExtra.Item3.TotalEmString.Equals("--:--") || !horaExtra.Item4.TotalEmString.Equals("--:--"))
                {
                    var herow = horaExtraDataSet.HoraExtra.NewHoraExtraRow();

                    herow.nome_funcionario = horaExtra.Item1.Nome;
                    herow.nome_loja = horaExtra.Item1.Loja.Nome;
                    herow.hora_100 = horaExtra.Item2.TotalEmString;
                    herow.hora_normal = horaExtra.Item3.TotalEmString;
                    herow.faltas = horaExtra.Item4.TotalEmString;
                    herow.mes_referencia = horaExtra.Item5.ToString("MM/yyyy");

                    horaExtraDataSet.HoraExtra.AddHoraExtraRow(herow);
                }
            }

            ReportParameter descTipoHoraExtraParameter = null;
            ReportDataSource reportDataSource = new ReportDataSource("DataSetHoraExtraFalta", horaExtraDataSet.Tables[0]);

            var first = listaOrdenadaPorLoja.Where(w => w.Item3.TipoHoraExtra != null).FirstOrDefault();
            if (first != null)
                descTipoHoraExtraParameter = new ReportParameter("TipoHoraExtraDescricao", first.Item3.TipoHoraExtra.Descricao);

            ReportViewerUtil.ConfiguraReportViewer(HoraExtraFaltaReportViewer,
                "VandaModaIntimaWpf.View.FolhaPagamento.Relatorios.RelatorioHoraExtraFaltas.rdlc",
                reportDataSource,
                null);

            if (descTipoHoraExtraParameter != null)
                HoraExtraFaltaReportViewer.LocalReport.SetParameters(descTipoHoraExtraParameter);

            HoraExtraFaltaReportViewer.LocalReport.Refresh();
            HoraExtraFaltaReportViewer.RefreshReport();
        }
    }
}

using Microsoft.Reporting.WinForms;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VandaModaIntimaWpf.View.Interfaces;

namespace VandaModaIntimaWpf.View.PontoEletronico
{
    /// <summary>
    /// Interaction logic for TelaRelatorioPontosPorMes.xaml
    /// </summary>
    public partial class TelaRelatorioPontosPorMes : Window
    {
        private IConfiguraReportViewer configuraReport;
        private ReportDataSource reportDataSource;
        public TelaRelatorioPontosPorMes()
        {
            InitializeComponent();
        }

        public TelaRelatorioPontosPorMes(ISession session, Model.Funcionario funcionario, DateTime data)
        {
            InitializeComponent();
            configuraReport = new ConfiguraReportViewerPonto(session);
            reportDataSource = configuraReport.ConfigurarReportDataSource(funcionario, data).Result;
        }

        private void PontoReportViewer_Load(object sender, EventArgs e)
        {
            configuraReport.Configurar(PontoReportViewer, reportDataSource, "VandaModaIntimaWpf.View.PontoEletronico.Relatorios.PontoEletronicoRelatorioFuncionario.rdlc");
        }
    }
}

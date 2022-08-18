using System;
using System.Windows;
using VandaModaIntimaWpf.View.Ferias.Relatorios;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.View.Ferias
{
    /// <summary>
    /// Interaction logic for TelaComunicacaoDeFerias.xaml
    /// </summary>
    public partial class TelaComunicacaoDeFerias : Window
    {
        public TelaComunicacaoDeFerias()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
            //FeriasReport.Owner = this;
        }

        public TelaComunicacaoDeFerias(Model.Ferias ferias)
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
            //FeriasReport.Owner = this;

            FeriasDataSet feriasDataSet = new FeriasDataSet();

            var report = new ComunicacaoFerias();

            var frow = feriasDataSet.ferias.NewferiasRow();

            frow.cnpj = Convert.ToUInt64(ferias.Funcionario.Loja.Cnpj).ToString(@"00\.000\.000\/0000\-00");
            frow.nomefuncionario = ferias.Funcionario.Nome;
            frow.cpffuncionario = Convert.ToUInt64(ferias.Funcionario.Cpf).ToString(@"000\.000\.000\-00");
            frow.dataretorno = ferias.Fim.AddDays(1).ToString("dd 'de' MMMM 'de' yyyy");
            frow.inicioferias = ferias.Inicio.ToString("dd 'de' MMMM 'de' yyyy");
            frow.fimferias = ferias.Fim.ToString("dd 'de' MMMM 'de' yyyy");
            frow.inicioaquisitivo = ferias.InicioAquisitivo.ToString("dd 'de' MMMM 'de' yyyy");
            frow.fimaquisitivo = ferias.FimAquisitivo.ToString("dd 'de' MMMM 'de' yyyy");

            if (ferias.Funcionario.Loja.Cnpj.StartsWith("17361873"))
            {
                frow.razaosocial = "JOSÉ FELIPE SILVA BORGES";
            }
            else if (ferias.Funcionario.Loja.Cnpj.StartsWith("13938943"))
            {
                frow.razaosocial = "JOSÉ LUCAS SILVA BORGES";
            }
            else
            {
                frow.razaosocial = "MARIA VANDA SILVA BORGES";
            }

            feriasDataSet.ferias.AddferiasRow(frow);

            report.SetDataSource(feriasDataSet);
            //FeriasReport.ViewerCore.EnableDrillDown = false;
            //FeriasReport.ViewerCore.ReportSource = report;
        }
    }
}

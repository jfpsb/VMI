using System;
using System.Collections.ObjectModel;
using System.Windows;
using VandaModaIntimaWpf.View.FolhaPagamento.Relatorios;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    /// <summary>
    /// Interaction logic for TelaRelatorioHoraExtra.xaml
    /// </summary>
    public partial class TelaRelatorioHoraExtra : Window
    {
        public TelaRelatorioHoraExtra()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
        }

        public TelaRelatorioHoraExtra(ObservableCollection<Tuple<Model.Funcionario, TimeSpan, TimeSpan, DateTime>> listaHoraExtra)
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();

            RelatorioHoraExtraDataSet horaExtraDataSet = new RelatorioHoraExtraDataSet();

            foreach (var horaExtra in listaHoraExtra)
            {
                if (horaExtra.Item2.TotalSeconds > 0 || horaExtra.Item3.TotalSeconds > 0)
                {
                    var herow = horaExtraDataSet.HoraExtra.NewHoraExtraRow();

                    herow.nome_funcionario = horaExtra.Item1.Nome;
                    herow.nome_loja = horaExtra.Item1.Loja.Nome;
                    herow.hora_100 = horaExtra.Item2.ToString("hh\\:mm");
                    herow.hora_55 = horaExtra.Item3.ToString("hh\\:mm");
                    herow.mes_referencia = horaExtra.Item4.ToString("MM/yyyy");

                    horaExtraDataSet.HoraExtra.AddHoraExtraRow(herow);
                }
            }

            var report = new RelatorioHoraExtra();
            report.SetDataSource(horaExtraDataSet);
            HoraExtraReport.ViewerCore.EnableDrillDown = false;
            HoraExtraReport.ViewerCore.ReportSource = report;
        }
    }
}

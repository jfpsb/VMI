using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using VandaModaIntimaWpf.View.FolhaPagamento.Relatorios;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    /// <summary>
    /// Interaction logic for TelaRelatorioHoraExtra.xaml
    /// </summary>
    public partial class TelaRelatorioHoraExtraFaltas : Window
    {
        public TelaRelatorioHoraExtraFaltas()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
        }

        public TelaRelatorioHoraExtraFaltas(ObservableCollection<Tuple<Model.Funcionario, TimeSpan, TimeSpan, TimeSpan, DateTime>> listaHoraExtra)
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;

            InitializeComponent();

            HoraExtraFaltasDataSet horaExtraDataSet = new HoraExtraFaltasDataSet();

            var listaOrdenadaPorLoja = listaHoraExtra.OrderBy(o => o.Item1.Loja.Cnpj);

            foreach (var horaExtra in listaOrdenadaPorLoja)
            {
                if (horaExtra.Item2.TotalSeconds > 0 || horaExtra.Item3.TotalSeconds > 0 || horaExtra.Item4.TotalSeconds > 0)
                {
                    var herow = horaExtraDataSet.HoraExtra.NewHoraExtraRow();

                    herow.nome_funcionario = horaExtra.Item1.Nome;
                    herow.nome_loja = horaExtra.Item1.Loja.Nome;
                    herow.hora_100 = horaExtra.Item2.ToString("hh\\:mm");
                    herow.hora_55 = horaExtra.Item3.ToString("hh\\:mm");
                    herow.faltas = horaExtra.Item4.ToString("hh\\:mm");
                    herow.mes_referencia = horaExtra.Item5.ToString("MM/yyyy");

                    horaExtraDataSet.HoraExtra.AddHoraExtraRow(herow);
                }
            }

            var report = new RelatorioHoraExtraFaltas();
            report.SetDataSource(horaExtraDataSet);
            HoraExtraReport.ViewerCore.EnableDrillDown = false;
            HoraExtraReport.ViewerCore.ReportSource = report;
        }
    }
}

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

        public TelaRelatorioHoraExtraFaltas(ObservableCollection<Tuple<Model.Funcionario, string, string, string, DateTime>> listaHoraExtra)
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;

            InitializeComponent();

            HoraExtraFaltasDataSet horaExtraDataSet = new HoraExtraFaltasDataSet();

            var listaOrdenadaPorLoja = listaHoraExtra.OrderBy(o => o.Item1.Loja.Cnpj);

            foreach (var horaExtra in listaOrdenadaPorLoja)
            {
                if (!horaExtra.Item2.Equals("00:00") || !horaExtra.Item3.Equals("00:00") || !horaExtra.Item4.Equals("00:00"))
                {
                    var herow = horaExtraDataSet.HoraExtra.NewHoraExtraRow();

                    herow.nome_funcionario = horaExtra.Item1.Nome;
                    herow.nome_loja = horaExtra.Item1.Loja.Nome;
                    herow.hora_100 = horaExtra.Item2;
                    herow.hora_55 = horaExtra.Item3;
                    herow.faltas = horaExtra.Item4;
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

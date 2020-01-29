using System.Collections.Generic;
using System.Windows;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.ViewModel.SQL;

namespace VandaModaIntimaWpf.View.SQL
{
    /// <summary>
    /// Interaction logic for ImportarExportarSQL.xaml
    /// </summary>
    public partial class ExportarSQL : Window
    {
        public ExportarSQL()
        {
            InitializeComponent();
        }
        public ExportarSQL(object exportarSQLViewModel)
        {
            InitializeComponent();
            DataContext = exportarSQLViewModel;
        }
    }
}

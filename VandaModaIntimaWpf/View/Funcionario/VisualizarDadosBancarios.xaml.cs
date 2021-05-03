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

namespace VandaModaIntimaWpf.View.Funcionario
{
    /// <summary>
    /// Interaction logic for VisualizarDadosBancarios.xaml
    /// </summary>
    public partial class VisualizarDadosBancarios : Window
    {
        public VisualizarDadosBancarios()
        {
            InitializeComponent();
        }

        public VisualizarDadosBancarios(Model.Funcionario funcionario)
        {
            InitializeComponent();
            ChavePixDataGrid.ItemsSource = funcionario.ChavesPix;
            ContasDataGrid.ItemsSource = funcionario.ContasBancarias;
        }

        private void ChavePixMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selecionado = ChavePixDataGrid.SelectedItem as Model.ChavePix;
            Clipboard.SetText(selecionado.Chave);
        }
    }
}

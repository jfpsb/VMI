using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VandaModaIntimaWpf.View.Contagem
{
    /// <summary>
    /// Interaction logic for AdicionarContagemProduto.xaml
    /// </summary>
    public partial class AdicionarContagemProduto : Window
    {
        public AdicionarContagemProduto()
        {
            InitializeComponent();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down && DataGridProdutos.Items.Count > 0)
            {
                DataGridProdutos.Focus();
                var dataGridCellInfo = new DataGridCellInfo(DataGridProdutos.Items[0], DataGridProdutos.Columns[0]);
                DataGridProdutos.CurrentCell = dataGridCellInfo;
                DataGridProdutos.SelectedItem = dataGridCellInfo.Item;
                DataGridProdutos.BeginEdit();
            }

            DataGridContagens.ScrollIntoView(DataGridContagens.Items.GetItemAt(DataGridContagens.Items.Count - 1));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataGridContagens.ScrollIntoView(DataGridContagens.Items.GetItemAt(DataGridContagens.Items.Count - 1));
        }
    }
}

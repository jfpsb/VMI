using System.Windows;
using System.Windows.Controls;

namespace VandaModaIntimaWpf.View.Produto
{
    /// <summary>
    /// Interaction logic for EditarProduto.xaml
    /// </summary>
    public partial class EditarProduto : Window, ICloseable
    {
        public EditarProduto()
        {
            InitializeComponent();
        }

        private void BtnCadastrarFornecedor_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ContextMenu contextMenu = button.ContextMenu;
            contextMenu.PlacementTarget = button;
            contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            contextMenu.IsOpen = true;
        }
    }
}
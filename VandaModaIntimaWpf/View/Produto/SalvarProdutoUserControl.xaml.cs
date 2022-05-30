using System.Windows;
using System.Windows.Controls;

namespace VandaModaIntimaWpf.View.Produto
{
    public partial class SalvarProdutoUserControl : UserControl
    {
        public SalvarProdutoUserControl()
        {
            InitializeComponent();
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            TxtCodBarra.Focus();
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

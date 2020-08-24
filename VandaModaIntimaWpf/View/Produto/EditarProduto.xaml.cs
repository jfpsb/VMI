using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VandaModaIntimaWpf.ViewModel;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CodigoFornecedorDataGrid.Items.Count > 0)
            {
                var border = VisualTreeHelper.GetChild(CodigoFornecedorDataGrid, 0) as Decorator;
                if (border != null)
                {
                    var scroll = border.Child as ScrollViewer;
                    if (scroll != null) scroll.ScrollToEnd();
                }
            }
        }
    }
}
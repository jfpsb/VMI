using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Produto
{
    /// <summary>
    /// Interaction logic for CadastrarProduto.xaml
    /// </summary>
    public partial class CadastrarProduto : Window
    {
        public CadastrarProduto()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((ACadastrarViewModel<Model.Produto>)DataContext).ResultadoSalvar();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtDescricao.Focus();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //TODO:ScrollToEnd DataGrid
            //if (CodigoFornecedorDataGrid.Items.Count > 0)
            //{
            //    var border = VisualTreeHelper.GetChild(CodigoFornecedorDataGrid, 0) as Decorator;
            //    if (border != null)
            //    {
            //        var scroll = border.Child as ScrollViewer;
            //        if (scroll != null) scroll.ScrollToEnd();
            //    }
            //}
        }
    }
}

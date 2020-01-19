using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Produto
{
    /// <summary>
    /// Interaction logic for EditarProduto.xaml
    /// </summary>
    public partial class EditarProduto : Window, ICloseable, IResultReturnable
    {
        public EditarProduto()
        {
            InitializeComponent();
        }
        public EditarProduto(object Id)
        {
            InitializeComponent();
            ((IEditarViewModel)DataContext).PassaId(Id);
        }
        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((IEditarViewModel)DataContext).EdicaoComSucesso();
        }
    }
}
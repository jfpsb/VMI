using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Fornecedor
{
    /// <summary>
    /// Interaction logic for EditarFornecedor.xaml
    /// </summary>
    public partial class EditarFornecedor : Window, ICloseable, IResultReturnable
    {
        public EditarFornecedor()
        {
            InitializeComponent();
        }
        public EditarFornecedor(object Id)
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

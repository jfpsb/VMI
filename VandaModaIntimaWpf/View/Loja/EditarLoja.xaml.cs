using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Loja
{
    /// <summary>
    /// Interaction logic for EditarLoja.xaml
    /// </summary>
    public partial class EditarLoja : Window, ICloseable, IResultReturnable
    {
        public EditarLoja()
        {
            InitializeComponent();
        }
        public EditarLoja(object Id)
        {
            InitializeComponent();
            ((IEditarViewModel)DataContext).PassaId(Id);
        }
        public void Window_Closing(object sender, CancelEventArgs e)
        {
            DialogResult = ((IEditarViewModel)DataContext).EdicaoComSucesso();
        }
    }
}

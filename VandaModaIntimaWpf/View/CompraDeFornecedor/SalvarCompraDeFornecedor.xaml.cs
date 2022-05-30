using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.CompraDeFornecedor
{
    /// <summary>
    /// Interaction logic for SalvarCompraDeFornecedor.xaml
    /// </summary>
    public partial class SalvarCompraDeFornecedor : ACadastrarView
    {
        private Window window;
        public SalvarCompraDeFornecedor()
        {
            InitializeComponent();
        }

        private void TelaSalvarCompraDeFornecedor_Loaded(object sender, RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            //Application.Current.MainWindow = window;
            
            //window.Width = 600;
            //window.Height =450;
            //SetWidth(600);
            //SetHeight(450);
            //Application.Current.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Title = "Salvar Compra De Fornecedor";
            //window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Icon = GetIcon("/Resources/Compras_De_Fornecedor.png");
            window.Closing += Window_Closing;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            window.DialogResult = ((ACadastrarViewModel<Model.CompraDeFornecedor>)DataContext).ResultadoSalvar();
        }
    }
}

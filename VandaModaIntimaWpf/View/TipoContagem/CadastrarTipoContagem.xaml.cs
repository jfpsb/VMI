using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.TipoContagem
{
    //TODO: Fazer View base para telas de cadastro
    /// <summary>
    /// Interaction logic for CadastrarTipoContagem.xaml
    /// </summary>
    public partial class CadastrarTipoContagem : Window
    {
        public CadastrarTipoContagem()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext != null)
                DialogResult = ((ACadastrarViewModel)DataContext).ResultadoSalvar();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtNome.Focus();
        }
    }
}

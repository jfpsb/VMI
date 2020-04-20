using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Contagem
{
    /// <summary>
    /// Interaction logic for CadastrarContagem.xaml
    /// </summary>
    public partial class CadastrarContagem : Window
    {
        public CadastrarContagem()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((ACadastrarViewModel)DataContext).ResultadoSalvar();
        }
    }
}

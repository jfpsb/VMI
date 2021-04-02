using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.RecebimentoCartao
{
    /// <summary>
    /// Interaction logic for CadastrarRecebimentoCartao.xaml
    /// </summary>
    public partial class CadastrarRecebimentoCartao : Window
    {
        public CadastrarRecebimentoCartao()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((ACadastrarViewModel<Model.RecebimentoCartao>)DataContext).ResultadoSalvar();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

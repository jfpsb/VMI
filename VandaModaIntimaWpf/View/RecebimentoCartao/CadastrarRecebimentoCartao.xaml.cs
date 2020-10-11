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
            var result = ((ACadastrarViewModel<Model.RecebimentoCartao>)DataContext).ResultadoSalvar();
            if (result != null)
                DialogResult = true;
            else
                DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

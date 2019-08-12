using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Marca
{
    /// <summary>
    /// Interaction logic for PesquisarMarca.xaml
    /// </summary>
    public partial class PesquisarMarca : Window, IMessageable
    {
        public PesquisarMarca()
        {
            InitializeComponent();
        }

        public void MensagemDeAviso(string mensagem)
        {
            MessageBox.Show(mensagem, "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void MensagemDeErro(string mensagem)
        {
            MessageBox.Show(mensagem, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public MessageBoxResult MensagemSimOuNao(string mensagem, string caption)
        {
            return MessageBox.Show(mensagem, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

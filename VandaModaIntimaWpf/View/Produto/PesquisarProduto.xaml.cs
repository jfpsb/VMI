using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Produto
{
    public partial class PesquisarProduto : Window, IMessageable
    {
        public PesquisarProduto()
        {
            InitializeComponent();
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PesquisarProduto_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Fecha sessao
            ((IPesquisarViewModel)DataContext).DisposeSession();
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
    }
}

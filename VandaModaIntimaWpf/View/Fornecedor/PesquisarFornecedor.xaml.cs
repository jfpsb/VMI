using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Fornecedor
{
    public partial class PesquisarFornecedor : Window, IMessageable
    {
        public PesquisarFornecedor()
        {
            InitializeComponent();
        }
        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Close();
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

        private void TelaPesquisarFornecedor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Fecha sessao
            ((IPesquisarViewModel)DataContext).DisposeSession();
        }

    }
}

using System.Windows;
using VandaModaIntimaWpf.View;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    /// <summary>
    /// Interaction logic for EditarFornecedor.xaml
    /// </summary>
    public partial class EditarFornecedor : Window, ICloseable, IMessageable, IResultReturnable
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
        public void MensagemDeAviso(string mensagem)
        {
            MessageBox.Show(mensagem, "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void MensagemDeErro(string mensagem)
        {
            MessageBox.Show(mensagem, "Erro ao Editar", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public MessageBoxResult MensagemSimOuNao(string mensagem, string caption)
        {
            return MessageBox.Show(mensagem, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((IEditarViewModel)DataContext).EdicaoComSucesso();
        }
    }
}

using System.Windows;
using VandaModaIntimaWpf.ViewModel;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto.Produto;

namespace VandaModaIntimaWpf.View.Produto
{
    /// <summary>
    /// Interaction logic for EditarProduto.xaml
    /// </summary>
    public partial class EditarProduto : Window, ICloseable, IMessageable
    {
        public EditarProduto()
        {
            InitializeComponent();
        }

        public EditarProduto(object Id)
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((IEditarViewModel)DataContext).EdicaoComSucesso();
        }
    }
}

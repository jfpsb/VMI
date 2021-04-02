using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Fornecedor
{
    /// <summary>
    /// Interaction logic for CadastrarFornecedorManualmente.xaml
    /// </summary>
    public partial class CadastrarFornecedorManualmente : Window
    {
        public CadastrarFornecedorManualmente()
        {
            InitializeComponent();
        }

        private void TelaCadastrarFornecedor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((ACadastrarViewModel<Model.Fornecedor>)DataContext).ResultadoSalvar();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtCnpj.Focus();
        }
    }
}

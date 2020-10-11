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
            var result = ((ACadastrarViewModel<Model.Fornecedor>)DataContext).ResultadoSalvar();
            if (result != null)
                DialogResult = true;
            else
                DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtCnpj.Focus();
        }
    }
}

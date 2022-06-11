using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Fornecedor
{
    /// <summary>
    /// Interaction logic for CadastrarFornecedorManualmente.xaml
    /// </summary>
    public partial class SalvarFornecedor : ACadastrarView
    {
        public SalvarFornecedor()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtCnpj.Focus();
        }

        private void TxtCnpj_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TxtCnpj.CaretIndex = TxtCnpj.Text.Length;
        }
    }
}

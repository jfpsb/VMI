using System.Windows;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    /// <summary>
    /// Interaction logic for AdicionarAdiantamento.xaml
    /// </summary>
    public partial class AdicionarAdiantamento : ACadastrarView
    {
        public AdicionarAdiantamento()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtValor.Focus();
        }
    }
}

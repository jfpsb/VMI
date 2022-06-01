using System.Windows;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    /// <summary>
    /// Interaction logic for AdicionarBonus.xaml
    /// </summary>
    public partial class AdicionarBonus : ACadastrarView
    {
        public AdicionarBonus()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtDescricao.Focus();
        }
    }
}

using System.Windows;

namespace VandaModaIntimaWpf.View.Contagem
{
    /// <summary>
    /// Interaction logic for CadastrarContagem.xaml
    /// </summary>
    public partial class CadastrarContagem : ACadastrarView
    {
        public CadastrarContagem()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CmbLoja.Focus();
        }
    }
}

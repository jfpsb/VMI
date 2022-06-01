using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.TipoContagem
{
    //TODO: Fazer View base para telas de cadastro
    /// <summary>
    /// Interaction logic for CadastrarTipoContagem.xaml
    /// </summary>
    public partial class CadastrarTipoContagem : ACadastrarView
    {
        public CadastrarTipoContagem()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtNome.Focus();
        }
    }
}

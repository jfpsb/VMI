using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.TipoContagem
{
    //TODO: Fazer View base para telas de cadastro
    /// <summary>
    /// Interaction logic for CadastrarTipoContagem.xaml
    /// </summary>
    public partial class CadastrarTipoContagem : Window
    {
        public CadastrarTipoContagem()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //TODO: Ver forma de arrumar isso pra não ter código repetido em cada WindowClosing de cada tela
            DialogResult = ((ACadastrarViewModel<Model.TipoContagem>)DataContext).ResultadoSalvar();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtNome.Focus();
        }
    }
}

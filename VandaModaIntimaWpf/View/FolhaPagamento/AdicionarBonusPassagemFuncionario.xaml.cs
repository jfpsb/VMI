using System.Windows;
using VandaModaIntimaWpf.ViewModel.FolhaPagamento;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    /// <summary>
    /// Interaction logic for AdicionarBonusPassagemFuncionario.xaml
    /// </summary>
    public partial class AdicionarBonusPassagemFuncionario : Window
    {
        public AdicionarBonusPassagemFuncionario()
        {
            InitializeComponent();
        }

        private void TelaAdicionarBonusPassagem_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Fecha sessao
            ((AdicionarBonusPassagemFuncionarioVM)DataContext).DisposeSession();
        }
    }
}

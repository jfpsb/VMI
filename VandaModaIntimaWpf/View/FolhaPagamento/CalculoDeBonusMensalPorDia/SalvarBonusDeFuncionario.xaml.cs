using System.Windows;
using VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia;
using VandaModaIntimaWpf.ViewModel.Interfaces;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    /// <summary>
    /// Interaction logic for AdicionarBonusPassagemFuncionario.xaml
    /// </summary>
    public partial class SalvarBonusDeFuncionario : Window
    {
        public SalvarBonusDeFuncionario()
        {
            InitializeComponent();
        }

        private void TelaAdicionarBonusPassagem_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IRequestClose)
            {
                (DataContext as IRequestClose).RequestClose += (_, __) =>
                {
                    Close();
                };
            }
        }
    }
}

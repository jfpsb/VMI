using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    /// <summary>
    /// Interaction logic for SalvarFaltas.xaml
    /// </summary>
    public partial class AdicionarFaltas : Window
    {
        public AdicionarFaltas()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            DialogResult = ((ACadastrarViewModel<Model.Faltas>)DataContext).ResultadoSalvar();
        }
    }
}

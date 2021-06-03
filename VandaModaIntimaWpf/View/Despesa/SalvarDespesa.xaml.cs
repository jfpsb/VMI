using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Despesa
{
    /// <summary>
    /// Interaction logic for SalvarDespesa.xaml
    /// </summary>
    public partial class SalvarDespesa : Window
    {
        public SalvarDespesa()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((ACadastrarViewModel<Model.Despesa>)DataContext).ResultadoSalvar();
        }
    }
}

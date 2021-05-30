using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Representante
{
    /// <summary>
    /// Interaction logic for SalvarRepresentante.xaml
    /// </summary>
    public partial class SalvarRepresentante : Window
    {
        public SalvarRepresentante()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((ACadastrarViewModel<Model.Representante>)DataContext).ResultadoSalvar();
        }
    }
}

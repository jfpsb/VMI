using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    /// <summary>
    /// Interaction logic for AdicionarHoraExtra.xaml
    /// </summary>
    public partial class AdicionarHoraExtra : Window, ICloseable
    {
        public AdicionarHoraExtra()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            var result = ((ACadastrarViewModel<Model.HoraExtra>)DataContext).ResultadoSalvar();

            if (result != null)
                DialogResult = true;
            else
                DialogResult = false;
        }
    }
}

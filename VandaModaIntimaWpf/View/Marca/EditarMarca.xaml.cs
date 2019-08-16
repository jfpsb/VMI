using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.ViewModel.Marca
{
    /// <summary>
    /// Interaction logic for EditarMarca.xaml
    /// </summary>
    public partial class EditarMarca : Window
    {
        public EditarMarca()
        {
            InitializeComponent();
        }

        public EditarMarca(object Id)
        {
            InitializeComponent();
            ((IEditarViewModel)DataContext).PassaId(Id);
        }
    }
}

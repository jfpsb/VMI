using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Contagem
{
    /// <summary>
    /// Interaction logic for EditarContagem.xaml
    /// </summary>
    public partial class EditarContagem : Window
    {
        public EditarContagem()
        {
            InitializeComponent();
        }

        public EditarContagem(object Id)
        {
            InitializeComponent();
            ((IEditarViewModel)DataContext).PassaId(Id);
        }

        public void Window_Closing(object sender, CancelEventArgs e)
        {
            DialogResult = ((IEditarViewModel)DataContext).EdicaoComSucesso();
        }
    }
}

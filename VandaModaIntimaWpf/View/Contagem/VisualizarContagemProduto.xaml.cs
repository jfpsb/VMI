using System;
using System.Collections.Generic;
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
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.ViewModel.Contagem;
using ContagemModel = VandaModaIntimaWpf.Model.Contagem;

namespace VandaModaIntimaWpf.View.Contagem
{
    /// <summary>
    /// Interaction logic for VisualizarContagemProduto.xaml
    /// </summary>
    public partial class VisualizarContagemProduto : Window
    {
        public VisualizarContagemProduto()
        {
            InitializeComponent();
        }

        public VisualizarContagemProduto(ContagemModel contagem)
        {
            InitializeComponent();
            DataContext = new VisualizarContagemProdutoVM(contagem);
        }
    }
}

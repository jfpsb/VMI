using System.Windows.Controls;
using VandaModaIntimaWpf.View;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public partial class PesquisarProduto : APesquisarView
    {
        public PesquisarProduto()
        {
            InitializeComponent();

            ((MenuItem)this.contextMenu.Items[2]).DataContext = DataContext;
        }
    }
}

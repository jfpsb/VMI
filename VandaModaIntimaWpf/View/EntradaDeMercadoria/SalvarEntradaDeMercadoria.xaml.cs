using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.EntradaDeMercadoria
{
    /// <summary>
    /// Interaction logic for SalvarEntradaDeMercadoria.xaml
    /// </summary>
    public partial class SalvarEntradaDeMercadoria : ACadastrarView
    {
        public SalvarEntradaDeMercadoria()
        {
            InitializeComponent();
            ProdutoListView.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
        }

        private void ItemContainerGenerator_StatusChanged(object sender, System.EventArgs e)
        {
            if (ProdutoListView.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            {
                int index = ProdutoListView.SelectedIndex;

                if (index >= 0)
                {
                    ListViewItem item = ProdutoListView.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;

                    if (item != null)
                        item.Focus();
                }
            }
        }

        private void TxtTermoProduto_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    int zindex = Panel.GetZIndex(ProdutoListView);

                    if (zindex == 1)
                    {
                        ProdutoListView.Focus();
                        ProdutoListView.SelectedIndex = 0;
                    }
                    break;
            }
        }

        private void ProdutoListView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if (ProdutoListView.SelectedIndex == 0)
                        TxtPesquisaProduto.Focus();
                    break;
            }
        }

        private void TxtQuantidade_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Dispatcher.BeginInvoke(new Action(() => textBox.SelectAll()));
        }
    }
}

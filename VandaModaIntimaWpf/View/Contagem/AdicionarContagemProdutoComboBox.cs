using System.Windows.Controls;
using System.Windows.Input;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.View.Contagem
{
    class AdicionarContagemProdutoComboBox : ComboBox
    {
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            ProdutoModel p = SelectedItem as ProdutoModel;
            Text = p.Descricao;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Enter && SelectedItem != null)
            {
                ProdutoModel p = SelectedItem as ProdutoModel;
                Text = p.Descricao;
            }

            if (e.Key == Key.Down)
            {
                var comboBox = e.Source as ComboBox;
                comboBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }
    }
}

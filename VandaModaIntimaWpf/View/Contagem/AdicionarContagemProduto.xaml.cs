using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace VandaModaIntimaWpf.View.Contagem
{
    /// <summary>
    /// Interaction logic for AdicionarContagemProduto.xaml
    /// </summary>
    public partial class AdicionarContagemProduto : Window
    {
        public AdicionarContagemProduto()
        {
            InitializeComponent();
        }

        private void ComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            comboBox.IsDropDownOpen = true;
        }

        private void ComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                var comboBoxItem = e.OriginalSource as FrameworkElement;
                var comboBox = ItemsControl.ItemsControlFromItemContainer(comboBoxItem) as ComboBox;
                comboBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }
    }
}

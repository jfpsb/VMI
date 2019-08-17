using System.Windows;

namespace VandaModaIntimaWpf.View
{
    public partial class TelaApagarDialog : Window
    {
        public TelaApagarDialog(string text, string caption)
        {
            InitializeComponent();
            Title = caption;
            txtText.Content = text;
        }
        private void BtnSim_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        private void BtnNao_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

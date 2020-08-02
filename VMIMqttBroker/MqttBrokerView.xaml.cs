using System.Windows;

namespace VMIMqttBroker
{
    /// <summary>
    /// Interaction logic for MqttBrokerView.xaml
    /// </summary>
    public partial class MqttBrokerView : Window
    {
        public MqttBrokerView()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((ICloseable)DataContext).Close();
        }
    }
}

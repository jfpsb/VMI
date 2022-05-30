using System;
using System.Windows;

namespace VandaModaIntimaWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        private void MenuItemAbrirEditar_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(sender.ToString());
        }
    }
}

using System.Windows;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    public class OpenView : IOpenViewService
    {
        public void Show(object viewModel)
        {
            var view = new Window();
            view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            view.Content = viewModel;
            //view.SizeToContent = SizeToContent.WidthAndHeight;
            view.Show();
        }

        public bool? ShowDialog(object viewModel)
        {
            var view = new Window();
            view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            view.Content = viewModel;
            //view.SizeToContent = SizeToContent.WidthAndHeight;
            return view.ShowDialog();
        }
    }
}

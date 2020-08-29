using System.Windows;

namespace VandaModaIntimaWpf.ViewModel.Services.Interfaces
{
    public interface IMessageBoxService
    {
        MessageBoxResult Show(
            string messageBoxText,
            string caption = "",
            MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage icon = MessageBoxImage.Information,
            MessageBoxResult defaultResult = MessageBoxResult.None,
            MessageBoxOptions options = MessageBoxOptions.None);
    }
}

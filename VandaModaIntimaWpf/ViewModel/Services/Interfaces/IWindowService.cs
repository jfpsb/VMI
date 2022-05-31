using System;

namespace VandaModaIntimaWpf.ViewModel.Services.Interfaces
{
    public interface IWindowService
    {
        void Show(object viewModel, Action<bool?> onCloseCallback);
        bool? ShowDialog(object viewModel, Action<bool?> onCloseCallback);
    }
}

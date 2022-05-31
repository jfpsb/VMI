using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    public class WindowService : IWindowService
    {
        private static Dictionary<Type, Type> mapeamentoViewModel = new Dictionary<Type, Type>();

        public static void RegistrarWindow<TView, TViewModel>()
        {
            mapeamentoViewModel.Add(typeof(TViewModel), typeof(TView));
        }
        public void Show(object viewModel, Action<bool?> onCloseCallback)
        {
            throw new NotImplementedException();
        }

        public bool? ShowDialog(object viewModel, Action<bool?> onCloseCallback)
        {
            var typeView = mapeamentoViewModel[viewModel.GetType()];
            return ShowDialogInternal(typeView, viewModel, onCloseCallback);
        }

        private bool? ShowDialogInternal(Type typeView, object viewModel, Action<bool?> onCloseCallback)
        {
            var window = Activator.CreateInstance(typeView) as Window;

            CancelEventHandler eventHandler = null;
            eventHandler = (s, e) =>
            {
                onCloseCallback(window.DialogResult);
                window.Closing -= eventHandler;
            };

            window.Closing += eventHandler;
            window.DataContext = viewModel;
            return window.ShowDialog();
        }
    }
}

using System.Windows;
using VandaModaIntimaWpf.ViewModel;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;

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

            var viewModel = new VandaModaIntimaVM(new AbreTelaPesquisaService());
            var view = new VandaModaIntima()
            {
                DataContext = viewModel
            };
            view.Show();
        }
    }
}

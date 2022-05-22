namespace VandaModaIntimaWpf.ViewModel.Services.Interfaces
{
    public interface IOpenViewService
    {
        void Show(object viewModel);
        bool? ShowDialog(object viewModel);
    }
}

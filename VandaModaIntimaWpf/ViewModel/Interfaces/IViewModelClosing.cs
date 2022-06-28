namespace VandaModaIntimaWpf.ViewModel.Interfaces
{
    /// <summary>
    /// Usada em ViewModels para disponibilizar um método na View que será usado no evento OnClosing ou OnClose da View.
    /// </summary>
    public interface IViewModelClosing
    {
        void OnClosing();
    }
}

namespace VandaModaIntimaWpf.ViewModel.Services.Interfaces
{
    public interface IFileDialogService
    {
        string ShowFolderBrowserDialog();
        string ShowFileBrowserDialog(string filtroExtensao = "");
    }
}

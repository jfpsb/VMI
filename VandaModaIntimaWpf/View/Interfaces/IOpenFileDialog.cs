namespace VandaModaIntimaWpf.View.Interfaces
{
    public interface IOpenFileDialog
    {
        string OpenFileDialog(string filtro = "");
        string[] OpenFileDialogMultiSelect(string filtro = "");
    }
}

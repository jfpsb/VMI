namespace VandaModaIntimaWpf.ViewModel
{
    interface IPesquisarViewModel
    {
        void AbrirCadastrar(object parameter);
        void AbrirEditar(object parameter);
        void AbrirApagarMsgBox(object parameter);
        bool IsCommandButtonEnabled(object parameter);
        void ChecarItensMarcados(object parameter);
        void ApagarMarcados(object parameter);
        void ExportarExcel(object parameter);
        void GetItems(string termo);
        void DisposeSession();
    }
}

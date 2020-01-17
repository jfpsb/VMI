using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel
{
    interface IPesquisarViewModel
    {
        void AbrirCadastrar(object parameter);
        void AbrirEditar(object parameter);
        void AbrirApagarMsgBox(object parameter);
        void AbrirAjuda(object parameter);
        void ChecarItensMarcados(object parameter);
        void ApagarMarcados(object parameter);
        void ExportarExcel(object parameter);
        void ImportarExcel(object parameter);
        void GetItems(string termo);
        void DisposeSession();
        bool IsThreadLocked();
        void SetStatusBarItemApagado();
        void SetStatusBarAguardandoExcel();
        void SetStatusBarExportadoComSucesso();
        Task ResetarStatusBar();
    }
}

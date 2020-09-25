using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel
{
    interface IPesquisarVM
    {
        void AbrirCadastrar(object parameter);
        void AbrirEditar(object parameter);
        void AbrirApagarMsgBox(object parameter);
        void AbrirAjuda(object parameter);
        void AbrirImprimir(object parameter);
        void ChecarItensMarcados(object parameter);
        void ApagarMarcados(object parameter);
        void ExportarExcel(object parameter);
        void ImportarExcel(object parameter);
        void PesquisaItens(string termo);
        void DisposeSession();
        bool IsThreadLocked();
        bool Editavel(object parameter);
        void SetStatusBarItemDeletado(string mensagem);
        void SetStatusBarAguardandoExcel();
        void SetStatusBarAguardando();
        void SetStatusBarExportadoComSucesso();
        void SetStatusBarErro(string mensagem);
        void AbrirExportarSQL(object parameter);
        Task ResetarStatusBar();
    }
}

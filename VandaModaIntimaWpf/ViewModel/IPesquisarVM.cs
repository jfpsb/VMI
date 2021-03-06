﻿using System.Threading.Tasks;

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
        void AbrirExportarSQL(object parameter);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel
{
    interface IPesquisarViewModel
    {
        void AbrirCadastrarNovo(object parameter);
        void AbrirEditar(object parameter);
        void AbrirApagarMsgBox(object parameter);
        bool IsCommandButtonEnabled(object parameter);
        void GetItems(string termo);
        void DisposeSession();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel
{
    interface ICadastrarViewModel
    {
        void Cadastrar(object parameter);
        bool ValidaModel(object parameter);
        void ResetaPropriedades();
    }
}

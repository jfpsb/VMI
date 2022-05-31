using System.ComponentModel;

namespace VandaModaIntimaWpf.ViewModel
{
    interface ICadastrarVM
    {
        void Salvar(object parameter);
        bool ValidacaoSalvar(object parameter);
        bool ResultadoSalvar();
    }
}

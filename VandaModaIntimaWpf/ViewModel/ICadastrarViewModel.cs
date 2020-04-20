using System.ComponentModel;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel
{
    interface ICadastrarViewModel
    {
        void Salvar(object parameter);
        bool ValidacaoSalvar(object parameter);
        void ResetaPropriedades();
        void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e);
        Task SetStatusBarSucesso(string mensagem);
        void SetStatusBarAguardando(string mensagem);
        void SetStatusBarErro(string mensagem);
    }
}

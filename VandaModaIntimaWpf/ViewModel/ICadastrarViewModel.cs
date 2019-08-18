using System.ComponentModel;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel
{
    interface ICadastrarViewModel
    {
        void Cadastrar(object parameter);
        bool ValidaModel(object parameter);
        void ResetaPropriedades();
        void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e);
        Task SetStatusBarSucesso();
        void SetStatusBarAguardando();
        void SetStatusBarErro();
    }
}

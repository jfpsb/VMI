using System.ComponentModel;

namespace VandaModaIntimaWpf.ViewModel
{
    interface ICadastrarVM
    {
        void Salvar(object parameter);
        bool ValidacaoSalvar(object parameter);
        void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e);
        void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e);
    }
}

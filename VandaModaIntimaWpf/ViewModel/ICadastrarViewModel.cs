using System.ComponentModel;

namespace VandaModaIntimaWpf.ViewModel
{
    interface ICadastrarViewModel
    {
        void Salvar(object parameter);
        bool ValidacaoSalvar(object parameter);
        void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e);
    }
}

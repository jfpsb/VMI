using NHibernate;
using System.Windows;
using VandaModaIntimaWpf.Resources;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class EditarLojaViewModel : CadastrarLojaViewModel
    {
        public EditarLojaViewModel(ISession session) : base(session) { }

        public override async void Salvar(object parameter)
        {
            if (Loja.Matriz != null && Loja.Matriz.Nome.Equals(StringResource.GetString("matriz_nao_selecionada")))
                Loja.Matriz = null;

            _result = await daoLoja.Merge(Loja);

            if (_result)
            {
                await SetStatusBarSucesso($"Loja {Loja.Cnpj} Atualizada Com Sucesso");
            }
            else
            {
                SetStatusBarErro("Erro ao Atualizar Loja");
            }
        }

        public new LojaModel Loja
        {
            get { return lojaModel; }
            set
            {
                lojaModel = value;
                OnPropertyChanged("Loja");
                OnPropertyChanged("MatrizComboBox");
            }
        }

        public LojaModel MatrizComboBox
        {
            get
            {
                if (Loja.Matriz == null)
                {
                    Loja.Matriz = new LojaModel(StringResource.GetString("matriz_nao_selecionada"));
                }

                return Loja.Matriz;
            }

            set
            {
                Loja.Matriz = value;
                OnPropertyChanged("MatrizComboBox");
            }
        }
    }
}

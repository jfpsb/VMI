using NHibernate;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class EditarLojaVM : CadastrarLojaVM
    {
        public EditarLojaVM(ISession session, IMessageBoxService messageBoxService) : base(session, messageBoxService, true)
        {
            viewModelStrategy = new EditarLojaVMStrategy();

            if (Entidade.Matriz == null)
                Entidade.Matriz = Matrizes[0];
        }
        public LojaModel MatrizComboBox
        {
            get
            {
                if (Entidade.Matriz == null)
                {
                    Entidade.Matriz = new LojaModel(GetResource.GetString("matriz_nao_selecionada"));
                }

                return Entidade.Matriz;
            }

            set
            {
                Entidade.Matriz = value;
                OnPropertyChanged("MatrizComboBox");
            }
        }
    }
}

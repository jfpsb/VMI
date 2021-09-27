using NHibernate;
using System.Collections.ObjectModel;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class EditarProdutoVM : CadastrarProdutoVM
    {
        public EditarProdutoVM(ISession session, ProdutoModel produto, IMessageBoxService messageBoxService) : base(session, messageBoxService, true)
        {
            viewModelStrategy = new EditarProdutoVMStrategy();
            Entidade = produto;
            ProdutoGrades = new ObservableCollection<ProdutoGrade>(Entidade.Grades);
            ProdutoGradeComposicaoPreco = ProdutoGrades[0];
        }
    }
}

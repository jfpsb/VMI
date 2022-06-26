using NHibernate;
using System.Collections.ObjectModel;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class EditarProdutoVM : CadastrarProdutoVM
    {
        public EditarProdutoVM(ISession session, Model.Produto produto) : base(session, true)
        {
            viewModelStrategy = new EditarProdutoVMStrategy();
            Entidade = produto;
            ProdutoGrades = new ObservableCollection<ProdutoGrade>(Entidade.Grades);
            ProdutoGradeComposicaoPreco = ProdutoGrades[0];
        }
    }
}

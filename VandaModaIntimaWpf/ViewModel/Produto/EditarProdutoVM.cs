using System.Collections.ObjectModel;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.ViewModel.Util;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class EditarProdutoVM : CadastrarProdutoVM
    {
        public EditarProdutoVM() : base()
        {
            viewModelStrategy = new EditarProdutoVMStrategy();
            var parametrosVM = ViewModelParameterHandler.Instance.GetParametros(GetType());
            Entidade = parametrosVM["Entidade"] as Model.Produto;
            ProdutoGrades = new ObservableCollection<ProdutoGrade>(Entidade.Grades);
            ProdutoGradeComposicaoPreco = ProdutoGrades[0];
        }
    }
}

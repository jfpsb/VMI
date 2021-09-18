using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class EditarProdutoVM : CadastrarProdutoVM
    {
        private ProdutoModel produtoOriginal;
        public EditarProdutoVM(ISession session, ProdutoModel produto, IMessageBoxService messageBoxService) : base(session, messageBoxService, true)
        {
            viewModelStrategy = new EditarProdutoVMStrategy();
            Entidade = produto;
            produtoOriginal = produto.Clone() as ProdutoModel;
            ProdutoGrade.Produto = Entidade;
            ProdutoGrades = new ObservableCollection<ProdutoGrade>(Entidade.Grades);
            ProdutoGradeComposicaoPreco = ProdutoGrades[0];
        }
    }
}

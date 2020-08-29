using NHibernate;
using System.Collections.Generic;
using VandaModaIntimaWpf.View.Produto;
using VandaModaIntimaWpf.View.SQL;
using VandaModaIntimaWpf.ViewModel.Produto;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using VandaModaIntimaWpf.ViewModel.SQL;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    public class AbrePelaTelaPesqProdutoService : IAbrePelaTelaPesquisaService<Model.Produto>
    {
        public void AbrirAjuda()
        {
            AjudaProduto ajudaProduto = new AjudaProduto();
            ajudaProduto.ShowDialog();
        }

        public bool? AbrirCadastrar(ISession session)
        {
            CadastrarProdutoVM cadastrarProdutoViewModel = new CadastrarProdutoVM(session, new MessageBoxService());

            CadastrarProduto cadastrar = new CadastrarProduto()
            {
                DataContext = cadastrarProdutoViewModel
            };

            return cadastrar.ShowDialog();
        }

        public bool? AbrirEditar(Model.Produto clone, ISession session)
        {
            EditarProdutoVM editarProdutoViewModel = new EditarProdutoVM(session, clone, new MessageBoxService());

            EditarProduto editar = new EditarProduto()
            {
                DataContext = editarProdutoViewModel
            };

            return editar.ShowDialog();
        }

        public void AbrirExportarSQL(IList<Model.Produto> entidades)
        {
            ExportarSQL importarExportarSQL = new ExportarSQL(new ExportarSQLProduto());
            importarExportarSQL.ShowDialog();
        }
    }
}

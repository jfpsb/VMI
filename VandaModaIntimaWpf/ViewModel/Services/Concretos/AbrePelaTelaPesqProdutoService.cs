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
            CadastrarProdutoVM cadastrarProdutoViewModel = new CadastrarProdutoVM(session, new MessageBoxService(), false);

            SalvarProduto cadastrar = new SalvarProduto()
            {
                DataContext = cadastrarProdutoViewModel
            };

            return cadastrar.ShowDialog();
        }

        public bool? AbrirEditar(Model.Produto clone, ISession session)
        {
            EditarProdutoVM editarProdutoViewModel = new EditarProdutoVM(session, clone, new MessageBoxService());

            SalvarProduto editar = new SalvarProduto()
            {
                DataContext = editarProdutoViewModel
            };

            return editar.ShowDialog();
        }

        public void AbrirExportarSQL(IList<Model.Produto> entidades, ISession session)
        {
            ExportarSQLProduto viewModel = new ExportarSQLProduto(entidades, session);
            ExportarSQL importarExportarSQL = new ExportarSQL();
            importarExportarSQL.DataContext = viewModel;
            importarExportarSQL.ShowDialog();
        }

        public void AbrirImprimir(IList<Model.Produto> lista)
        {
            TelaRelatorioProduto telaRelatorioProduto = new TelaRelatorioProduto(lista);
            telaRelatorioProduto.Show();
        }
    }
}

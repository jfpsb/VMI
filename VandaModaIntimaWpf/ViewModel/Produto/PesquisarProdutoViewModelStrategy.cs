using NHibernate;
using System.Collections.Generic;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.View.Produto;
using VandaModaIntimaWpf.View.SQL;
using VandaModaIntimaWpf.ViewModel.SQL;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    class PesquisarProdutoViewModelStrategy : IPesquisarViewModelStrategy<ProdutoModel>
    {
        public void AbrirAjuda(object parameter)
        {
            AjudaProduto ajudaProduto = new AjudaProduto();
            ajudaProduto.ShowDialog();
        }

        public bool? AbrirCadastrar(object parameter, ISession session)
        {
            CadastrarProdutoViewModel cadastrarProdutoViewModel = new CadastrarProdutoViewModel(session);

            CadastrarProduto cadastrar = new CadastrarProduto()
            {
                DataContext = cadastrarProdutoViewModel
            };

            return cadastrar.ShowDialog();
        }

        public bool? AbrirEditar(ProdutoModel clone, ISession session)
        {
            EditarProdutoViewModel editarProdutoViewModel = new EditarProdutoViewModel(session)
            {
                Produto = clone
            };

            EditarProduto editar = new EditarProduto()
            {
                DataContext = editarProdutoViewModel
            };

            return editar.ShowDialog();
        }

        public void AbrirExportarSQL(object parameter, IList<ProdutoModel> entidades)
        {
            ExportarSQL importarExportarSQL = new ExportarSQL(new ExportarSQLProduto());
            importarExportarSQL.ShowDialog();
        }

        public string MensagemApagarEntidadeCerteza(ProdutoModel e)
        {
            return string.Format(GetResource.GetString("certeza_deletar_produto"), e.Descricao);
        }

        public string MensagemApagarMarcados()
        {
            return GetResource.GetString("deseja_apagar_produto_marcados");
        }

        public string MensagemEntidadeDeletada(ProdutoModel e)
        {
            return string.Format(GetResource.GetString("produto_deletado_com_sucesso"), e.Descricao);
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return GetResource.GetString("produto_nao_deletado");
        }

        public string MensagemEntidadesDeletadas()
        {
            return GetResource.GetString("produtos_deletados_com_sucesso");
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return GetResource.GetString("produtos_nao_deletados");
        }

        public void RestauraEntidade(ProdutoModel original, ProdutoModel backup)
        {
            original.Descricao = backup.Descricao;
            original.Preco = backup.Preco;
            original.Fornecedor = backup.Fornecedor;
            original.Marca = backup.Marca;
            original.Codigos = backup.Codigos;
        }

        public string TelaApagarCaption()
        {
            return GetResource.GetString("apagar_produtos");
        }
    }
}

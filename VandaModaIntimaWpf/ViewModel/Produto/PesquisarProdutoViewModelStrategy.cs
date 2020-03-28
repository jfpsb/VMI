﻿using System.Collections.Generic;
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

        public void AbrirCadastrar(object parameter)
        {
            CadastrarProduto cadastrar = new CadastrarProduto();
            cadastrar.ShowDialog();
        }

        public bool? AbrirEditar(ProdutoModel produto)
        {
            EditarProduto editar = new EditarProduto(produto.CodBarra);
            return editar.ShowDialog();
        }

        public void AbrirExportarSQL(object parameter, IList<ProdutoModel> entidades)
        {
            ExportarSQL importarExportarSQL = new ExportarSQL(new ExportarSQLProduto());
            importarExportarSQL.ShowDialog();
        }

        public string MensagemApagarEntidadeCerteza(ProdutoModel produto)
        {
            return $"Tem Certeza Que Deseja Apagar o Produto {produto.Descricao}?";
        }

        public string MensagemApagarMarcados()
        {
            return "Deseja Apagar os Produtos Marcados?";
        }

        public string MensagemEntidadeDeletada(ProdutoModel produto)
        {
            return $"Produto {produto.Descricao} Foi Deletado Com Sucesso";
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return "Produto Não Foi Deletado";
        }

        public string MensagemEntidadesDeletadas()
        {
            return "Produtos Apagados Com Sucesso";
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return "Erro Ao Apagar Fornecedores";
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
            return "Apagar Produto(s)";
        }
    }
}

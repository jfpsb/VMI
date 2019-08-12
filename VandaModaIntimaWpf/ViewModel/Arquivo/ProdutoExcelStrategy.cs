using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto.Produto;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor.Fornecedor;
using MarcaModel = VandaModaIntimaWpf.Model.Marca.Marca;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    class ProdutoExcelStrategy : IExportarExcelStrategy
    {
        public void EscreveDados(Worksheet Worksheet, object l)
        {
            var lista = (IList<ProdutoModel>)l;

            for (int i = 0; i < lista.Count; i++)
            {
                Worksheet.Cells[i + 2, ProdutoModel.Colunas.CodBarra] = lista[i].Cod_Barra;
                Worksheet.Cells[i + 2, ProdutoModel.Colunas.Descricao] = lista[i].Descricao;
                Worksheet.Cells[i + 2, ProdutoModel.Colunas.Preco] = lista[i].Preco;
                Worksheet.Cells[i + 2, ProdutoModel.Colunas.Fornecedor] = "NÃO HÁ FORNECEDOR";
                Worksheet.Cells[i + 2, ProdutoModel.Colunas.Marca] = "NÃO HÁ MARCA";

                if (lista[i].Fornecedor != null)
                    Worksheet.Cells[i + 2, ProdutoModel.Colunas.Fornecedor] = lista[i].Fornecedor.Nome;

                if (lista[i].Marca != null)
                    Worksheet.Cells[i + 2, ProdutoModel.Colunas.Marca] = lista[i].Marca.Nome;
            }
        }

        public string[] GetColunas()
        {
            return new ProdutoModel().GetColunas();
        }

        public bool LeEInsereDados(Worksheet Worksheet)
        {
            ProdutoModel produtoModel = new ProdutoModel();
            FornecedorModel fornecedorModel = new FornecedorModel();
            MarcaModel marcaModel = new MarcaModel();
            IList<ProdutoModel> produtos = new List<ProdutoModel>();

            Range range = Worksheet.UsedRange;

            int rows = range.Rows.Count;
            int cols = range.Columns.Count;

            if (cols != 6)
                return false;

            for (int i = 0; i < rows; i++)
            {
                ProdutoModel produto = new ProdutoModel();

                var cod_barra = ((Range)Worksheet.Cells[i + 2, 1]).Value;
                var descricao = ((Range)Worksheet.Cells[i + 2, 2]).Value;
                var preco = ((Range)Worksheet.Cells[i + 2, 3]).Value;
                var fornecedor = ((Range)Worksheet.Cells[i + 2, 4]).Value;
                var marca = ((Range)Worksheet.Cells[i + 2, 5]).Value;
                var cod_barra_fornecedor = ((Range)Worksheet.Cells[i + 2, 6]).Value;

                if (cod_barra == null || descricao == null || preco == null)
                {
                    continue;
                }

                produto.Cod_Barra = cod_barra.ToString();
                produto.Descricao = descricao.ToString();
                produto.Preco = preco;

                if (!string.IsNullOrEmpty(fornecedor))
                    produto.Fornecedor = fornecedorModel.ListarPorId(fornecedor);

                if (!string.IsNullOrEmpty(marca))
                    produto.Marca = marcaModel.ListarPorId(marca);

                if (!string.IsNullOrEmpty(cod_barra_fornecedor))
                {
                    string[] codigos = cod_barra_fornecedor.Split(',');

                    foreach (string s in codigos)
                    {
                        produto.Codigos.Add(s);
                    }
                }

                produtos.Add(produto);
            }

            return produtoModel.Salvar(produtos);
        }
    }
}

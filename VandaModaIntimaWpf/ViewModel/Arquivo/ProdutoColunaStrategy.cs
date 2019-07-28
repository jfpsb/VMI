using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto.Produto;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    class ProdutoColunaStrategy : IExportarExcelStrategy
    {
        public void EscreveDados(Worksheet Worksheet, object l)
        {
            string[] colunas = new ProdutoModel().GetColunas();
            var lista = (IList<ProdutoModel>)l;

            for (int i = 0; i < colunas.Length; i++)
            {
                Worksheet.Cells[1, i + 1] = colunas[i];
                Worksheet.Cells[1, i + 1].Font.Bold = true;
                Worksheet.Cells[1, i + 1].Font.Size = 14;
                Worksheet.Cells[1, i + 1].Interior.Color = Color.LightGray;
                Worksheet.Cells[1, i + 1].Borders.Color = Color.Black;
            }

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

                Worksheet.Columns.AutoFit();
                //Worksheet.Columns.AutoFit();
            }
        }
    }
}

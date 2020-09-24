using Microsoft.Office.Interop.Excel;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    public class ProdutoExcelStrategy : IExcelStrategy
    {
        private ISession _session;

        public ProdutoExcelStrategy(ISession session)
        {
            _session = session;
        }

        public void EscreveDados(Worksheet Worksheet, object l)
        {
            var lista = (IList<ProdutoModel>)l;

            for (int i = 0; i < lista.Count; i++)
            {
                Worksheet.Cells[i + 2, ProdutoModel.Colunas.CodBarra] = lista[i].CodBarra;
                Worksheet.Cells[i + 2, ProdutoModel.Colunas.Descricao] = lista[i].Descricao;
                Worksheet.Cells[i + 2, ProdutoModel.Colunas.Preco] = lista[i].Preco;
                Worksheet.Cells[i + 2, ProdutoModel.Colunas.Fornecedor] = "NÃO HÁ FORNECEDOR";
                Worksheet.Cells[i + 2, ProdutoModel.Colunas.Marca] = "NÃO HÁ MARCA";
                Worksheet.Cells[i + 2, ProdutoModel.Colunas.Ncm] = "NÃO HÁ NCM";

                if (lista[i].Fornecedor != null)
                    Worksheet.Cells[i + 2, ProdutoModel.Colunas.Fornecedor] = lista[i].Fornecedor.Nome;

                if (lista[i].Marca != null)
                    Worksheet.Cells[i + 2, ProdutoModel.Colunas.Marca] = lista[i].Marca.Nome;

                if (!string.IsNullOrEmpty(lista[i].Ncm))
                    Worksheet.Cells[i + 2, ProdutoModel.Colunas.Ncm] = lista[i].Ncm;

                if (lista[i].Codigos.Count > 0)
                {
                    string codigos = "";

                    foreach (string codigo in lista[i].Codigos)
                    {
                        codigos += $"{codigo},";
                    }

                    // Remove vírgula no final da string
                    codigos = codigos.Remove(codigos.Length - 1);

                    Worksheet.Cells[i + 2, ProdutoModel.Colunas.CodBarraFornecedor] = codigos;
                }
            }
        }

        public string[] GetColunas()
        {
            return new ProdutoModel().GetColunas();
        }

        public async Task<bool> LeEInsereDados(Worksheet Worksheet)
        {
            DAOProduto daoProduto = new DAOProduto(_session);
            DAOFornecedor daoFornecedor = new DAOFornecedor(_session);
            DAOMarca daoMarca = new DAOMarca(_session);
            IList<ProdutoModel> produtos = new List<ProdutoModel>();

            Range range = Worksheet.UsedRange;

            int rows = range.Rows.Count;
            int cols = range.Columns.Count;

            if (cols != 7)
                return false;

            for (int i = 0; i < rows; i++)
            {
                ProdutoModel produto = new ProdutoModel();

                var cod_barra = ((Range)Worksheet.Cells[i + 2, ProdutoModel.Colunas.CodBarra]).Value;
                var descricao = ((Range)Worksheet.Cells[i + 2, ProdutoModel.Colunas.Descricao]).Value;
                var preco = ((Range)Worksheet.Cells[i + 2, ProdutoModel.Colunas.Preco]).Value;
                var fornecedor = ((Range)Worksheet.Cells[i + 2, ProdutoModel.Colunas.Fornecedor]).Value;
                var marca = ((Range)Worksheet.Cells[i + 2, ProdutoModel.Colunas.Marca]).Value;
                var ncm = ((Range)Worksheet.Cells[i + 2, ProdutoModel.Colunas.Ncm]).Value;
                var cod_barra_fornecedor = ((Range)Worksheet.Cells[i + 2, ProdutoModel.Colunas.CodBarraFornecedor]).Value;

                if (cod_barra == null || descricao == null || preco == null)
                {
                    continue;
                }

                produto.CodBarra = cod_barra.ToString();
                produto.Descricao = descricao.ToString();
                produto.Preco = preco;

                if (!string.IsNullOrEmpty(fornecedor) && !fornecedor.ToString().Equals("NÃO POSSUI"))
                    produto.Fornecedor = await daoFornecedor.ListarPorIDOuNome(fornecedor);

                if (!string.IsNullOrEmpty(marca) && !marca.ToString().Equals("NÃO POSSUI"))
                    produto.Marca = await daoMarca.ListarPorId(marca);

                if (!string.IsNullOrEmpty(ncm) && !ncm.ToString().Equals("NÃO POSSUI"))
                    produto.Ncm = ncm.ToString();

                if (!string.IsNullOrEmpty(cod_barra_fornecedor))
                {
                    string[] codigos = cod_barra_fornecedor.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string s in codigos)
                    {
                        produto.Codigos.Add(s);
                    }
                }

                produtos.Add(produto);
            }

            return await daoProduto.Inserir(produtos);
        }
        public void ConfiguraColunas(Worksheet Worksheet)
        {
            Worksheet.Range["A1", "F1"].EntireColumn.AutoFit();
            Worksheet.Columns[ProdutoModel.Colunas.CodBarraFornecedor].ColumnWidth = 100;
            Worksheet.Columns[ProdutoModel.Colunas.CodBarraFornecedor].EntireColumn.WrapText = true;
        }
    }
}

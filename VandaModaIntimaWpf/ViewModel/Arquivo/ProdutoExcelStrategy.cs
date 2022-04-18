using Microsoft.Office.Interop.Excel;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    public class ProdutoExcelStrategy : AExcelStrategy
    {
        private ISession _session;
        private string[] _colunas = new[] { "Cód. de Barras", "Descrição", "Preço", "Fornecedor", "Marca", "NCM" };

        public ProdutoExcelStrategy(ISession session)
        {
            _session = session;
        }
        public override void AutoFitColunas(Worksheet Worksheet)
        {
            Worksheet.Range["A1", "F1"].EntireColumn.AutoFit();
        }
        public override void EscreveDados(Workbook workbook, params object[] l)
        {
            WorksheetContainer<ProdutoModel> wscontainer = (WorksheetContainer<ProdutoModel>)l[0];
            var lista = wscontainer.Lista;
            var worksheet = workbook.Worksheets.Add();
            worksheet.Name = wscontainer.Nome;
            worksheet.Cells.Font.Size = wscontainer.TamanhoFonteGeral;

            EscreveColunas(worksheet, _colunas, 1, 1);

            for (int i = 0; i < lista.Count; i++)
            {
                worksheet.Cells[i + 2, ProdutoModel.Colunas.CodBarra] = lista[i].CodBarra;
                worksheet.Cells[i + 2, ProdutoModel.Colunas.Descricao] = lista[i].Descricao;
                worksheet.Cells[i + 2, ProdutoModel.Colunas.Fornecedor] = "NÃO HÁ FORNECEDOR";
                worksheet.Cells[i + 2, ProdutoModel.Colunas.Marca] = "NÃO HÁ MARCA";
                worksheet.Cells[i + 2, ProdutoModel.Colunas.Ncm] = "NÃO HÁ NCM";

                if (lista[i].Fornecedor != null)
                    worksheet.Cells[i + 2, ProdutoModel.Colunas.Fornecedor] = lista[i].Fornecedor.Nome;

                if (lista[i].Marca != null)
                    worksheet.Cells[i + 2, ProdutoModel.Colunas.Marca] = lista[i].Marca.Nome;

                if (!string.IsNullOrEmpty(lista[i].Ncm))
                    worksheet.Cells[i + 2, ProdutoModel.Colunas.Ncm] = lista[i].Ncm;
            }
        }
        public override async Task LeEInsereDados(Workbook workbook)
        {
            DAOProduto daoProduto = new DAOProduto(_session);
            DAOFornecedor daoFornecedor = new DAOFornecedor(_session);
            DAOMarca daoMarca = new DAOMarca(_session);
            IList<ProdutoModel> produtos = new List<ProdutoModel>();

            var worksheet = workbook.Worksheets.Item[1];

            Range range = worksheet.UsedRange;

            int rows = range.Rows.Count;
            int cols = range.Columns.Count;

            if (cols != 7)
                throw new Exception("Planilha contém número de colunas inválido. O número correto de colunas da planilha de importação de produtos é sete.");

            for (int i = 0; i < rows; i++)
            {
                ProdutoModel produto = new ProdutoModel();

                var cod_barra = ((Range)worksheet.Cells[i + 2, ProdutoModel.Colunas.CodBarra]).Value;
                var descricao = ((Range)worksheet.Cells[i + 2, ProdutoModel.Colunas.Descricao]).Value;
                var fornecedor = ((Range)worksheet.Cells[i + 2, ProdutoModel.Colunas.Fornecedor]).Value;
                var marca = ((Range)worksheet.Cells[i + 2, ProdutoModel.Colunas.Marca]).Value;
                var ncm = ((Range)worksheet.Cells[i + 2, ProdutoModel.Colunas.Ncm]).Value;

                if (cod_barra == null || descricao == null)
                {
                    continue;
                }

                produto.CodBarra = cod_barra.ToString();
                produto.Descricao = descricao.ToString();

                if (!string.IsNullOrEmpty(fornecedor) && !fornecedor.ToString().Equals("NÃO POSSUI"))
                    produto.Fornecedor = await daoFornecedor.ListarPorIDOuNome(fornecedor);

                if (!string.IsNullOrEmpty(marca) && !marca.ToString().Equals("NÃO POSSUI"))
                    produto.Marca = await daoMarca.ListarPorId(marca);

                if (!string.IsNullOrEmpty(ncm) && !ncm.ToString().Equals("NÃO POSSUI"))
                    produto.Ncm = ncm.ToString();

                produtos.Add(produto);
            }

            await daoProduto.Inserir(produtos);
        }
    }
}

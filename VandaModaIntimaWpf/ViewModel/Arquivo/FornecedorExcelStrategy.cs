using Microsoft.Office.Interop.Excel;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    class FornecedorExcelStrategy : AExcelStrategy
    {
        private ISession _session;

        public FornecedorExcelStrategy(ISession session)
        {
            _session = session;
        }

        public override void AutoFitColunas(Worksheet Worksheet)
        {
            Worksheet.Columns.AutoFit();
        }
        public override void EscreveDados(Workbook workbook, params object[] l)
        {
            WorksheetContainer<FornecedorModel> wscontainer = (WorksheetContainer<FornecedorModel>)l[0];
            var lista = wscontainer.Lista;
            var worksheet = workbook.Worksheets.Add();
            worksheet.Name = wscontainer.Nome;
            worksheet.Cells.Font.Size = wscontainer.TamanhoFonteGeral;

            for (int i = 0; i < lista.Count; i++)
            {
                worksheet.Cells[i + 2, FornecedorModel.Colunas.Cnpj] = lista[i].Cnpj;
                worksheet.Cells[i + 2, FornecedorModel.Colunas.Nome] = lista[i].Nome;
                worksheet.Cells[i + 2, FornecedorModel.Colunas.NomeFantasia] = lista[i].Fantasia;
                worksheet.Cells[i + 2, FornecedorModel.Colunas.Email] = lista[i].Email;
                worksheet.Cells[i + 2, FornecedorModel.Colunas.Telefone] = lista[i].Telefone;
            }
        }
        public override async Task LeEInsereDados(Workbook workbook)
        {
            DAOFornecedor daoFornecedor = new DAOFornecedor(_session);
            IList<FornecedorModel> fornecedores = new List<FornecedorModel>();
            var worksheet = workbook.Worksheets.Add();

            Range range = worksheet.UsedRange;

            int rows = range.Rows.Count;
            int cols = range.Columns.Count;

            if (cols != 5)
                throw new Exception("Planilha contém número de colunas inválido. O número correto de colunas da planilha de importação de fornecedores é cinco.");

            for (int i = 0; i < rows; i++)
            {
                FornecedorModel fornecedor = new FornecedorModel();

                var cnpj = ((Range)worksheet.Cells[i + 2, FornecedorModel.Colunas.Cnpj]).Value;
                var nome = ((Range)worksheet.Cells[i + 2, FornecedorModel.Colunas.Nome]).Value;
                var nome_fantasia = ((Range)worksheet.Cells[i + 2, FornecedorModel.Colunas.NomeFantasia]).Value;
                var telefone = ((Range)worksheet.Cells[i + 2, FornecedorModel.Colunas.Telefone]).Value;
                var email = ((Range)worksheet.Cells[i + 2, FornecedorModel.Colunas.Email]).Value;

                if (cnpj == null || nome == null)
                    continue;

                fornecedor.Cnpj = cnpj.ToString();

                if (fornecedor.Cnpj.Length != 14)
                {
                    int diff = 14 - fornecedor.Cnpj.Length;

                    string append = "";

                    for (int j = 0; j < diff; j++)
                    {
                        append += "0";
                    }

                    append += fornecedor.Cnpj;
                    fornecedor.Cnpj = append;
                }

                fornecedor.Nome = nome.ToString();

                if (nome_fantasia != null)
                    fornecedor.Fantasia = nome_fantasia.ToString();

                if (telefone != null)
                    fornecedor.Telefone = telefone.toString();

                if (email != null)
                    fornecedor.Email = email.ToString();

                fornecedores.Add(fornecedor);
            }

            await daoFornecedor.Inserir(fornecedores);
        }
    }
}

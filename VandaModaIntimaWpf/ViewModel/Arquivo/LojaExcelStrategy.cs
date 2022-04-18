using Microsoft.Office.Interop.Excel;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    class LojaExcelStrategy : AExcelStrategy
    {
        private ISession _session;
        private string[] _colunas = new[] { "CNPJ", "Matriz", "Nome", "Telefone", "Endereço", "Inscrição Estadual" };

        public LojaExcelStrategy(ISession session)
        {
            _session = session;
        }

        public override void AutoFitColunas(Worksheet Worksheet)
        {
            Worksheet.Columns.AutoFit();
        }

        public override void EscreveDados(Workbook workbook, params object[] l)
        {
            WorksheetContainer<LojaModel> wscontainer = (WorksheetContainer<LojaModel>)l[0];
            var lista = wscontainer.Lista;
            var worksheet = workbook.Worksheets.Add();
            worksheet.Name = wscontainer.Nome;
            worksheet.Cells.Font.Size = wscontainer.TamanhoFonteGeral;

            EscreveColunas(worksheet, _colunas, 1, 1);

            for (int i = 0; i < lista.Count; i++)
            {
                worksheet.Cells[i + 2, LojaModel.Colunas.Cnpj] = $"'{lista[i].Cnpj}";
                worksheet.Cells[i + 2, LojaModel.Colunas.Matriz] = lista[i].Matriz?.Nome;
                worksheet.Cells[i + 2, LojaModel.Colunas.Nome] = lista[i].Nome;
                worksheet.Cells[i + 2, LojaModel.Colunas.Telefone] = lista[i].Telefone;
                worksheet.Cells[i + 2, LojaModel.Colunas.Endereco] = lista[i].Endereco;
                worksheet.Cells[i + 2, LojaModel.Colunas.InscricaoEstadual] = lista[i].InscricaoEstadual;
            }

            AutoFitColunas(worksheet);
        }
        public override async Task LeEInsereDados(Workbook workbook)
        {
            DAOLoja daoLoja = new DAOLoja(_session);
            IList<LojaModel> lojas = new List<LojaModel>();
            var worksheet = workbook.Worksheets.Add();

            Range range = worksheet.UsedRange;

            int rows = range.Rows.Count;
            int cols = range.Columns.Count;

            if (cols != 6)
                throw new Exception("Planilha contém número de colunas inválido. O número correto de colunas da planilha de importação de lojas é seis.");

            for (int i = 0; i < rows; i++)
            {
                LojaModel loja = new LojaModel();

                var cnpj = ((Range)worksheet.Cells[i + 2, LojaModel.Colunas.Cnpj]).Value;
                var matriz = ((Range)worksheet.Cells[i + 2, LojaModel.Colunas.Matriz]).Value;
                var nome = ((Range)worksheet.Cells[i + 2, LojaModel.Colunas.Nome]).Value;
                var telefone = ((Range)worksheet.Cells[i + 2, LojaModel.Colunas.Telefone]).Value;
                var endereco = ((Range)worksheet.Cells[i + 2, LojaModel.Colunas.Endereco]).Value;
                var inscricaoestadual = ((Range)worksheet.Cells[i + 2, LojaModel.Colunas.InscricaoEstadual]).Value;

                if (cnpj == null || nome == null)
                    continue;

                loja.Cnpj = cnpj.ToString();

                if (loja.Cnpj.Length != 14)
                {
                    int diff = 14 - loja.Cnpj.Length;

                    string append = "";

                    for (int j = 0; j < diff; j++)
                    {
                        append += "0";
                    }

                    append += loja.Cnpj;
                    loja.Cnpj = append;
                }

                loja.Nome = nome.ToString();

                if (nome != null)
                    loja.Nome = nome.ToString();

                if (telefone != null)
                    loja.Telefone = telefone.toString();

                if (endereco != null)
                    loja.Endereco = endereco.ToString();

                if (inscricaoestadual != null)
                    loja.InscricaoEstadual = inscricaoestadual.ToString();

                lojas.Add(loja);
            }

            await daoLoja.Inserir(lojas);
        }
    }
}

using Microsoft.Office.Interop.Excel;
using NHibernate;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    class MarcaExcelStrategy : AExcelStrategy
    {
        private ISession _session;

        public MarcaExcelStrategy(ISession session)
        {
            _session = session;
        }

        public override void AutoFitColunas(Worksheet Worksheet)
        {
            Worksheet.Columns.AutoFit();
        }

        public override void EscreveDados(Workbook workbook, params object[] l)
        {
            WorksheetContainer<MarcaModel> wscontainer = (WorksheetContainer<MarcaModel>)l[0];
            var lista = wscontainer.Lista;
            var worksheet = workbook.Worksheets.Add();
            worksheet.Name = wscontainer.Nome;
            worksheet.Cells.Font.Size = wscontainer.TamanhoFonteGeral;

            for (int i = 0; i < lista.Count; i++)
            {
                worksheet.Cells[i + 2, MarcaModel.Colunas.Nome] = lista[i].Nome;
            }
        }
        public override async Task<bool> LeEInsereDados(Workbook workbook)
        {
            DAOMarca daoMarca = new DAOMarca(_session);
            IList<MarcaModel> marcas = new List<MarcaModel>();
            var worksheet = workbook.Worksheets.Add();

            Range range = worksheet.UsedRange;

            int rows = range.Rows.Count;
            int cols = range.Columns.Count;

            if (cols != 1)
            {
                return false;
            }

            for (int i = 0; i < rows; i++)
            {
                MarcaModel marca = new MarcaModel();

                var nome = ((Range)worksheet.Cells[i + 2, 1]).Value;

                if (nome == null)
                    continue;

                marca.Nome = nome.ToString();

                marcas.Add(marca);
            }

            return await daoMarca.Inserir(marcas);
        }
    }
}

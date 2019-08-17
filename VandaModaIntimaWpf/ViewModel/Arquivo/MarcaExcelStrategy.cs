using Microsoft.Office.Interop.Excel;
using NHibernate;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    class MarcaExcelStrategy : IExcelStrategy
    {
        private ISession _session = SessionProvider.GetSession();
        public void EscreveDados(Worksheet Worksheet, object l)
        {
            var lista = (IList<MarcaModel>)l;

            for (int i = 0; i < lista.Count; i++)
            {
                Worksheet.Cells[i + 2, MarcaModel.Colunas.Nome] = lista[i].Nome;
            }
        }

        public string[] GetColunas()
        {
            return new MarcaModel().GetColunas();
        }

        public bool LeEInsereDados(Worksheet Worksheet)
        {
            DAOMarca daoMarca = new DAOMarca(_session);
            IList<MarcaModel> marcas = new List<MarcaModel>();

            Range range = Worksheet.UsedRange;

            int rows = range.Rows.Count;
            int cols = range.Columns.Count;

            if (cols != 1)
            {
                return false;
            }

            for (int i = 0; i < rows; i++)
            {
                MarcaModel marca = new MarcaModel();

                var nome = ((Range)Worksheet.Cells[i + 2, 1]).Value;

                if (nome == null)
                    continue;

                marca.Nome = nome.ToString();

                marcas.Add(marca);
            }

            return daoMarca.Inserir(marcas);
        }
    }
}

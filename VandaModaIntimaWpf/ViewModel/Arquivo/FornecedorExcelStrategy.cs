using Microsoft.Office.Interop.Excel;
using NHibernate;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    class FornecedorExcelStrategy : IExcelStrategy
    {
        private ISession _session = SessionProvider.GetSession();
        public void EscreveDados(Worksheet Worksheet, object l)
        {
            var lista = (IList<FornecedorModel>)l;

            for (int i = 0; i < lista.Count; i++)
            {
                Worksheet.Cells[i + 2, FornecedorModel.Colunas.CNPJ] = lista[i].Cnpj;
                Worksheet.Cells[i + 2, FornecedorModel.Colunas.Nome] = lista[i].Nome;
                Worksheet.Cells[i + 2, FornecedorModel.Colunas.NomeFantasia] = lista[i].NomeFantasia;
                Worksheet.Cells[i + 2, FornecedorModel.Colunas.Email] = lista[i].Email;
            }
        }

        public string[] GetColunas()
        {
            return new FornecedorModel().GetColunas();
        }

        public bool LeEInsereDados(Worksheet Worksheet)
        {
            DAOFornecedor daoFornecedor = new DAOFornecedor(_session);
            IList<FornecedorModel> fornecedores = new List<FornecedorModel>();

            Range range = Worksheet.UsedRange;

            int rows = range.Rows.Count;
            int cols = range.Columns.Count;

            if (cols != 4)
                return false;

            for (int i = 0; i < rows; i++)
            {
                FornecedorModel fornecedor = new FornecedorModel();

                var cnpj = ((Range)Worksheet.Cells[i + 2, 1]).Value;
                var nome = ((Range)Worksheet.Cells[i + 2, 2]).Value;
                var nome_fantasia = ((Range)Worksheet.Cells[i + 2, 3]).Value;
                var email = ((Range)Worksheet.Cells[i + 2, 4]).Value;

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
                    fornecedor.NomeFantasia = nome_fantasia.ToString();

                if (email != null)
                    fornecedor.Email = email.ToString();

                fornecedores.Add(fornecedor);
            }

            return daoFornecedor.Inserir(fornecedores);
        }
    }
}

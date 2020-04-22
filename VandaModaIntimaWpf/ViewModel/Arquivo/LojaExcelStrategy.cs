using Microsoft.Office.Interop.Excel;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    class LojaExcelStrategy : IExcelStrategy
    {
        private ISession _session;

        public LojaExcelStrategy(ISession session)
        {
            _session = session;
        }

        public void ConfiguraColunas(Worksheet Worksheet)
        {
            Worksheet.Columns.AutoFit();
        }

        public void EscreveDados(Worksheet Worksheet, object l)
        {
            var lista = (IList<LojaModel>)l;

            for (int i = 0; i < lista.Count; i++)
            {
                Worksheet.Cells[i + 2, LojaModel.Colunas.Cnpj] = lista[i].Cnpj;
                Worksheet.Cells[i + 2, LojaModel.Colunas.Matriz] = lista[i].Matriz?.Nome;
                Worksheet.Cells[i + 2, LojaModel.Colunas.Nome] = lista[i].Nome;
                Worksheet.Cells[i + 2, LojaModel.Colunas.Telefone] = lista[i].Telefone;
                Worksheet.Cells[i + 2, LojaModel.Colunas.Endereco] = lista[i].Endereco;
                Worksheet.Cells[i + 2, LojaModel.Colunas.InscricaoEstadual] = lista[i].InscricaoEstadual;
            }
        }

        public string[] GetColunas()
        {
            return new LojaModel().GetColunas();
        }

        public async Task<bool> LeEInsereDados(Worksheet Worksheet)
        {
            DAOLoja daoLoja = new DAOLoja(_session);
            IList<LojaModel> lojas = new List<LojaModel>();

            Range range = Worksheet.UsedRange;

            int rows = range.Rows.Count;
            int cols = range.Columns.Count;

            if (cols != 6)
                return false;

            for (int i = 0; i < rows; i++)
            {
                LojaModel loja = new LojaModel();

                var cnpj = ((Range)Worksheet.Cells[i + 2, LojaModel.Colunas.Cnpj]).Value;
                var matriz = ((Range)Worksheet.Cells[i + 2, LojaModel.Colunas.Matriz]).Value;
                var nome = ((Range)Worksheet.Cells[i + 2, LojaModel.Colunas.Nome]).Value;
                var telefone = ((Range)Worksheet.Cells[i + 2, LojaModel.Colunas.Telefone]).Value;
                var endereco = ((Range)Worksheet.Cells[i + 2, LojaModel.Colunas.Endereco]).Value;
                var inscricaoestadual = ((Range)Worksheet.Cells[i + 2, LojaModel.Colunas.InscricaoEstadual]).Value;

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

            return await daoLoja.Inserir(lojas);
        }
    }
}

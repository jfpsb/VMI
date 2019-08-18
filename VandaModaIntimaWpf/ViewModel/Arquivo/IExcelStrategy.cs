﻿using Microsoft.Office.Interop.Excel;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    public interface IExcelStrategy
    {
        string[] GetColunas();
        void EscreveDados(Worksheet Worksheet, object l);
        Task<bool> LeEInsereDados(Worksheet Worksheet);
    }
}

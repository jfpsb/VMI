using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Util
{
    public static class DateTimeUtil
    {
        /// <summary>
        /// Retorna o range de dias presentes no mês informado.
        /// </summary>
        /// <returns>Lista de dias no formato DateTime</returns>
        public static IEnumerable<DateTime> RetornaDiasEmMes(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                yield return new DateTime(year, month, day);
            }
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.Util
{
    public static class DateTimeUtil
    {
        /// <summary>
        /// Retorna o range de dias presentes no mês informado.
        /// </summary>
        /// <returns>Lista de dias no formato DateTime</returns>
        public static IEnumerable<DateTime> RetornaDiasEmMes(int year, int month, int start = 1, int end = 0)
        {
            int days = DateTime.DaysInMonth(year, month);

            if (start > days)
                start = 1;

            if (end != 0 && end <= days && end > 0)
                days = end;

            for (int day = start; day <= days; day++)
            {
                yield return new DateTime(year, month, day);
            }
        }

        /// <summary>
        /// Retorna o dia útil que o usuário deseja usando o arquivo de calendário para considerar feriados.
        /// Se quiser retornar o quinto dia útil, informe como dia o valor 5.
        /// </summary>
        /// <param name="ordemDia">Ordem do dia útil desejado pelo usuário.</param>
        /// <param name="mes">Mês para consultar dia útil</param>
        /// <param name="ano">Ano para consultar dia útil</param>
        /// <returns>Dia útil em DateTime</returns>
        public static DateTime RetornaDiaUtilComFeriado(int ordemDia, int mes, int ano)
        {
            if (ano < 2000)
                return new DateTime(ano, mes, 5);

            var arquivoFeriadosExiste = File.Exists($"Resources/Feriados/{ano}.json");

            if (!arquivoFeriadosExiste)
            {
                throw new FileNotFoundException("Não foi possível encontrar arquivo de calendário! Quinto dia útil retornado não irá considerar feriados, o que pode estar incorreto!");
            }

            var datasFeriadosJson = File.ReadAllText($"Resources/Feriados/{ano}.json");
            var datasFeriados = JsonConvert.DeserializeObject<DataFeriado[]>(datasFeriadosJson);

            int quintoDiaFlag = 0;

            foreach (var dia in RetornaDiasEmMes(ano, mes))
            {
                if (dia.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                var feriado = datasFeriados.FirstOrDefault(s => s.Date.Day == dia.Day && s.Date.Month == dia.Month);

                if (feriado != null)
                {
                    if (feriado.Type.ToLower().Equals("feriado nacional") || feriado.Type.ToLower().Equals("feriado estadual") || feriado.Type.ToLower().Equals("feriado municipal"))
                        continue;
                }

                quintoDiaFlag++;

                if (quintoDiaFlag == ordemDia)
                    return dia;
            }

            return new DateTime(ano, mes, 5);
        }

        /// <summary>
        /// Retorna o dia útil que o usuário deseja, sem considerar feriados através do arquivo de calendário.
        /// Se quiser retornar o quinto dia útil, informe como dia o valor 5.
        /// </summary>
        /// <param name="ordemDia">Ordem do dia útil desejado pelo usuário.</param>
        /// <param name="mes">Mês para consultar dia útil</param>
        /// <param name="ano">Ano para consultar dia útil</param>
        /// <returns>Dia útil em DateTime</returns>
        public static DateTime RetornaDiaUtilSemFeriado(int ordemDia, int mes, int ano)
        {
            if (ano < 2000)
                return new DateTime(ano, mes, 5);

            int quintoDiaFlag = 0;

            foreach (var dia in RetornaDiasEmMes(ano, mes))
            {
                if (dia.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                quintoDiaFlag++;

                if (quintoDiaFlag == ordemDia)
                    return dia;
            }

            return new DateTime(ano, mes, 5);
        }
    }
}

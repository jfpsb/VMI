using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using VandaModaIntimaWpf.Model;

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
        /// Retorna o dia útil que o usuário deseja. Se quiser retornar o quinto dia útil, informe como dia o valor 5.
        /// </summary>
        /// <param name="dia">Ordem do dia útil desejado pelo usuário.</param>
        /// <param name="mes">Mês para consultar dia útil</param>
        /// <param name="ano">Ano para consultar dia útil</param>
        /// <returns>Dia útil em DateTime</returns>
        public static DateTime RetornaDataUtil(int dia, int mes, int ano)
        {
            if (ano < 2000)
                return new DateTime(ano, mes, 5);

            if (!File.Exists($"Resources/Feriados/{ano}.json") && ano > 1999)
            {
                try
                {
                    string url = string.Format("https://api.calendario.com.br/?json=true&ano={0}&estado=MA&cidade=SAO_LUIS&token=amZwc2JfZmVsaXBlMkBob3RtYWlsLmNvbSZoYXNoPTE1NDcxMDY0NA", ano);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    WebResponse response = request.GetResponse();
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        File.WriteAllText($"Resources/Feriados/{ano}.json", reader.ReadToEnd());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            var datasFeriadosJson = File.ReadAllText($"Resources/Feriados/{ano}.json");
            var datasFeriados = JsonConvert.DeserializeObject<DataFeriado[]>(datasFeriadosJson);

            int quintoFlag = 0;

            foreach (var d in RetornaDiasEmMes(ano, mes))
            {
                if (d.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                var feriado = datasFeriados.FirstOrDefault(s => s.Date.Day == d.Day && s.Date.Month == d.Month);

                if (feriado != null)
                {
                    if (feriado.Type.ToLower().Equals("feriado nacional") || feriado.Type.ToLower().Equals("feriado estadual") || feriado.Type.ToLower().Equals("feriado municipal"))
                        continue;
                }

                quintoFlag++;

                if (quintoFlag == dia)
                    return d;
            }

            return new DateTime(ano, mes, 5);
        }
    }
}

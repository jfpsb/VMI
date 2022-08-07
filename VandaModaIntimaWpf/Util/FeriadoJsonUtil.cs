using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.Util
{
    public static class FeriadoJsonUtil
    {
        /// <summary>
        /// Retorna lista com todos os feriados do ano informado.
        /// </summary>
        /// <param name="ano">Ano a checar dias de feriado</param>
        /// <returns>Listagem com todos os datetimes dos feriados municipais, estaduais e federais do ano informado</returns>
        public static DataFeriado[] RetornaListagemDeFeriados(int ano)
        {
            if (!File.Exists($"Resources/Feriados/{ano}.json"))
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
            return JsonConvert.DeserializeObject<DataFeriado[]>(datasFeriadosJson);
        }

        /// <summary>
        /// Retorna se data informada é feriado.
        /// </summary>
        /// <param name="dia">Dia de data</param>
        /// <param name="mes">Mês de data</param>
        /// <param name="ano">Ano de data</param>
        /// <returns>Objeto DataFeriado se data for feriado, null se não for.</returns>
        public static DataFeriado IsDataFeriado(int dia, int mes, int ano)
        {
            var feriados = RetornaListagemDeFeriados(ano);
            return feriados.FirstOrDefault(f => f.Date.Day == dia && f.Date.Month == mes && f.Date.Year == ano);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.View.PontoEletronico
{
    /// <summary>
    /// Checa se há intervalos válidos. Se for dia útil e não há intervalos registrados então funcionário não registrou seus intervalos.
    /// Se é dia de sábado e domingo geralmente não há intervalos.
    /// Dia de feriado geralmente não há intervalos.
    /// </summary>
    public class ChecaSeIntervalosSaoValidosConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Model.PontoEletronico ponto = value as Model.PontoEletronico;
            DataFeriado dataFeriado = FeriadoJsonUtil.IsDataFeriado(ponto.Dia.Day, ponto.Dia.Month, ponto.Dia.Year);

            //Dia útil
            if (ponto.Dia.DayOfWeek != DayOfWeek.Sunday && ponto.Dia.DayOfWeek != DayOfWeek.Saturday)
            {
                //Não é feriado
                if (dataFeriado == null)
                {
                    return ponto.Intervalos != null && ponto.Intervalos.Count > 0;
                }
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

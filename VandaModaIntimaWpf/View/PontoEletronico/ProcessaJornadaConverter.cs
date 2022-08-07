using System;
using System.Globalization;
using System.Windows.Data;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.View.PontoEletronico
{
    public class ProcessaJornadaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Model.PontoEletronico ponto = value as Model.PontoEletronico;
            DataFeriado dataFeriado = FeriadoJsonUtil.IsDataFeriado(ponto.Dia.Day, ponto.Dia.Month, ponto.Dia.Year);
            TimeSpan limiteAtraso = new TimeSpan(0, 10, 0);
            TimeSpan zeroCargaHoraria = new TimeSpan(0, 0, 0);

            //Determina carga horária do dia do ponto
            TimeSpan cargaHoraria = ponto.Dia.DayOfWeek == DayOfWeek.Saturday ? new TimeSpan(4, 0, 0) : new TimeSpan(8, 0, 0);

            //Data não é feriado
            if (dataFeriado == null)
            {
                if (ponto.Dia.DayOfWeek == DayOfWeek.Sunday)
                {
                    if (ponto.Jornada == zeroCargaHoraria)
                        return "dianormal";

                    return "horaextra";
                }

                if (ponto.Jornada > cargaHoraria)
                {
                    var dif = ponto.Jornada - cargaHoraria;
                    if (dif > limiteAtraso)
                    {
                        return "horaextra";
                    }
                }
                else if (ponto.Jornada < cargaHoraria)
                {
                    var dif = cargaHoraria - ponto.Jornada;
                    if (dif > limiteAtraso)
                    {
                        return "falta";
                    }
                }
            }
            else
            {
                if (dataFeriado.Type.ToLower().Equals("feriado nacional")
                    || dataFeriado.Type.ToLower().Equals("feriado estadual")
                    || dataFeriado.Type.ToLower().Equals("feriado municipal"))
                {
                    if (ponto.Jornada > zeroCargaHoraria)
                        return "feriadotrabalhado";

                    return "feriado";
                }
            }

            return "dianormal";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

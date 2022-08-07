using System;
using System.Globalization;
using System.Windows.Data;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.View.PontoEletronico
{
    public class CalculoSaldoJornada : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Model.PontoEletronico ponto = value as Model.PontoEletronico;
            DataFeriado dataFeriado = FeriadoJsonUtil.IsDataFeriado(ponto.Dia.Day, ponto.Dia.Month, ponto.Dia.Year);
            TimeSpan cargaHoraria = ponto.Dia.DayOfWeek == DayOfWeek.Sunday ? new TimeSpan(4, 0, 0) : new TimeSpan(8, 0, 0);

            if (dataFeriado == null)
            {
                return RetornaSaldoDiaNormal(ponto, cargaHoraria);
            }

            if (dataFeriado.Type.ToLower().Equals("feriado nacional")
                    || dataFeriado.Type.ToLower().Equals("feriado estadual")
                    || dataFeriado.Type.ToLower().Equals("feriado municipal"))
            {
                return ponto.Jornada.ToString("hh\\:mm", new CultureInfo("pt-BR"));
            }

            return RetornaSaldoDiaNormal(ponto, cargaHoraria);
        }

        private static object RetornaSaldoDiaNormal(Model.PontoEletronico ponto, TimeSpan cargaHoraria)
        {
            string sinal = "";
            if (ponto.Jornada < cargaHoraria)
                sinal = "-";
            return $"Saldo: {sinal}{(ponto.Jornada - cargaHoraria).ToString("hh\\:mm", new CultureInfo("pt-BR"))}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

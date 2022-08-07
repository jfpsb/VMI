using System;
using System.Globalization;
using System.Windows.Data;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.View.PontoEletronico
{
    public class IsDataFeriadoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dia = (DateTime)value;
            DataFeriado dataFeriado = FeriadoJsonUtil.IsDataFeriado(dia.Day, dia.Month, dia.Year);

            if (dataFeriado != null && (dataFeriado.Type.ToLower().Equals("feriado nacional")
                    || dataFeriado.Type.ToLower().Equals("feriado estadual")
                    || dataFeriado.Type.ToLower().Equals("feriado municipal")))
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

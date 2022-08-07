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
            return dataFeriado != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

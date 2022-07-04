using System;
using System.Globalization;
using System.Windows.Data;

namespace VandaModaIntimaWpf.View.PontoEletronico
{
    public class PontoNullableDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "-- : --";

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

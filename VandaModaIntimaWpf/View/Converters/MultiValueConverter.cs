using System;
using System.Globalization;
using System.Windows.Data;

namespace VandaModaIntimaWpf.View.Converters
{
    public class PeriodoEmStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format("{0:dd/MM/yyyy} a {1:dd/MM/yyyy}", values[0], values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

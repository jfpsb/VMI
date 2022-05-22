using System;
using System.Globalization;
using System.Windows.Data;

namespace VandaModaIntimaWpf.View.Converters
{
    public class BooleanToYesOrNoStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valor = (bool)value;

            if (valor)
                return "SIM";

            return "NÃO";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

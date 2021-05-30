using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace VandaModaIntimaWpf.ViewModel.Converters
{
    public class CNPJValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.Equals(string.Empty))
                return string.Empty;

            long cnpj = long.Parse(Regex.Replace(value.ToString(), "[^0-9]", string.Empty));
            return string.Format(@"{0:00\.000\.000/0000-00}", cnpj);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.Equals(string.Empty))
                return string.Empty;


            int tamanhoMax = 14;
            string valorLimpo = Regex.Replace(value.ToString(), "[^0-9]", string.Empty).TrimStart('0');

            if (valorLimpo.Length > tamanhoMax)
            {
                return valorLimpo.Substring(0, tamanhoMax);
            }

            if (valorLimpo.Length == 0)
                return 0;

            string novaString = valorLimpo;

            if (novaString.Length < tamanhoMax)
            {
                novaString = novaString.PadLeft(tamanhoMax, '0');
            }

            return novaString;
        }
    }
}

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace VandaModaIntimaWpf.ViewModel.Converters
{
    public class CPFValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.Equals(string.Empty))
                return string.Empty;

            long cpf = long.Parse(Regex.Replace(value.ToString(), "[A-Za-z.-]", string.Empty));
            return string.Format(@"{0:000\.000\.000-00}", cpf);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.Equals(string.Empty))
                return string.Empty;


            int tamanhoMax = 11;
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

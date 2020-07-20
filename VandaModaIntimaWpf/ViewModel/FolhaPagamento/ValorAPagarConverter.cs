using System;
using System.Globalization;
using System.Windows.Data;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class ValorAPagarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value) < 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

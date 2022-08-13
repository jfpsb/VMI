using System;
using System.Globalization;
using System.Windows.Data;

namespace VandaModaIntimaWpf.View.Pix
{
    public class PossuiTxIdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Model.Pix.Pix pix = (Model.Pix.Pix)value;
            return pix.Txid != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

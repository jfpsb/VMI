using System;
using System.Globalization;
using System.Windows.Data;

namespace VandaModaIntimaWpf.View.Pix
{
    /// <summary>
    /// Processa de que forma o Pix foi transferido: por chave, qr code ou por dados bancários.
    /// </summary>
    public class ProcessaViaPixConverter : IValueConverter
    {
        /// <summary>
        /// Realiza checagem em objeto Pix.
        /// </summary>
        /// <param name="value">Objeto Pix</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>True se por QR Code, False se por chave estática, null se por dados bancários</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Model.Pix.Pix pix = (Model.Pix.Pix)value;

            if (pix.Chave == null)
                return null;

            return pix.Txid != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

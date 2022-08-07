using System;
using System.Globalization;
using System.Windows.Data;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.View.PontoEletronico
{
    public class DescreveDataFeriadoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dia = (DateTime)value;
            DataFeriado dataFeriado = FeriadoJsonUtil.IsDataFeriado(dia.Day, dia.Month, dia.Year);
            if (dataFeriado != null)
            {
                string descricao = $"{dataFeriado.Name}, {dataFeriado.Type}".ToLower();
                return string.Concat(descricao[0].ToString().ToUpper(), descricao.AsSpan(1).ToString());
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

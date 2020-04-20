﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace VandaModaIntimaWpf.ViewModel.RecebimentoCartao
{
    class FormatoDoubleDataGrid : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().Replace(",", ".");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().Replace(",", ".");
        }
    }
}
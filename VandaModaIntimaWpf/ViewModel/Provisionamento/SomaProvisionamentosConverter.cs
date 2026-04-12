using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace VandaModaIntimaWpf.ViewModel.Provisionamento
{
    public class SomaProvisionamentosConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cvg = value as CollectionViewGroup;
            var field = parameter as string;
            if (cvg == null || field == null)
                return null;

            return cvg.Items.Sum(r => (double)(r as EntidadeComCampo<Model.Provisionamento>).Entidade.ProvisionamentoTotal);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

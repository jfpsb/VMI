using Newtonsoft.Json.Converters;

namespace VandaModaIntimaWpf.Model.Converters
{
    public class TabelaINSSDataConverter : IsoDateTimeConverter
    {
        public TabelaINSSDataConverter()
        {
            DateTimeFormat = "MM/yyyy";
        }
    }
}

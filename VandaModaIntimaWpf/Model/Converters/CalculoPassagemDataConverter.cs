using Newtonsoft.Json.Converters;

namespace VandaModaIntimaWpf.Model.Converters
{
    public class CalculoPassagemDataConverter : IsoDateTimeConverter
    {
        public CalculoPassagemDataConverter()
        {
            DateTimeFormat = "dd/MM/yyyy";
        }
    }
}

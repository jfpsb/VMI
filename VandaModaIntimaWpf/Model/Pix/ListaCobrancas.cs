using Newtonsoft.Json;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model.Pix
{
    public class ListaCobrancas : ObservableObject
    {
        private Parametros _parametros;
        IList<Cobranca> _cobrancas = new List<Cobranca>();

        [JsonProperty(PropertyName = "cobs")]
        public IList<Cobranca> Cobrancas
        {
            get
            {
                return _cobrancas;
            }

            set
            {
                _cobrancas = value;
                OnPropertyChanged("Cobrancas");
            }
        }

        public Parametros Parametros
        {
            get
            {
                return _parametros;
            }

            set
            {
                _parametros = value;
                OnPropertyChanged("Parametros");
            }
        }
    }
}
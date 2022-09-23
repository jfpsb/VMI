using Newtonsoft.Json;

namespace SincronizacaoVMI.Model.Pix
{
    public class Loc : AModel, IModel
    {
        private int _id;
        private int _locId;
        private string _location;
        private string _tipoCob;
        private Cobranca _cobranca;

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        [JsonProperty(PropertyName = "id")]
        public virtual int LocId
        {
            get
            {
                return _locId;
            }

            set
            {
                _locId = value;
                OnPropertyChanged("LocId");
            }
        }

        public virtual string Location
        {
            get
            {
                return _location;
            }

            set
            {
                _location = value;
                OnPropertyChanged("Location");
            }
        }

        public virtual string TipoCob
        {
            get
            {
                return _tipoCob;
            }

            set
            {
                _tipoCob = value;
                OnPropertyChanged("TipoCob");
            }
        }

        public virtual Cobranca Cobranca
        {
            get
            {
                return _cobranca;
            }

            set
            {
                _cobranca = value;
                OnPropertyChanged("Cobranca");
            }
        }

        public virtual string GetContextMenuHeader => throw new System.NotImplementedException();

        public virtual void InicializaLazyLoad()
        {
            throw new System.NotImplementedException();
        }
    }
}

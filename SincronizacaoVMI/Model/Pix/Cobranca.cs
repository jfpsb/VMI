using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;

namespace SincronizacaoVMI.Model.Pix
{
    public class Cobranca : AModel, IModel
    {
        private int _id;
        private string _txid;
        private Loja _loja;
        private Calendario _calendario;
        private Valor _valor;
        private Loc _loc;
        private QRCode _qrCode;
        private int _revisao;
        private string _status;
        private string _chave;
        private string _location;
        private DateTime _pagoEm;

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

        [JsonProperty(PropertyName = "calendario")]
        public virtual Calendario Calendario
        {
            get
            {
                return _calendario;
            }

            set
            {
                _calendario = value;
                OnPropertyChanged("Calendario");
            }
        }

        [JsonProperty(PropertyName = "valor")]
        public virtual Valor Valor
        {
            get
            {
                return _valor;
            }

            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }

        [JsonProperty(PropertyName = "txid")]
        public virtual string Txid
        {
            get
            {
                return _txid;
            }

            set
            {
                _txid = value;
                OnPropertyChanged("Txid");
            }
        }

        [JsonProperty(PropertyName = "revisao")]
        public virtual int Revisao
        {
            get
            {
                return _revisao;
            }

            set
            {
                _revisao = value;
                OnPropertyChanged("Revisao");
            }
        }

        [JsonProperty(PropertyName = "status")]
        public virtual string Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        [JsonProperty(PropertyName = "chave")]
        public virtual string Chave
        {
            get
            {
                return _chave;
            }

            set
            {
                _chave = value;
                OnPropertyChanged("Chave");
            }
        }

        [JsonProperty(PropertyName = "location")]
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

        [JsonProperty(PropertyName = "loc")]
        public virtual Loc Loc
        {
            get
            {
                return _loc;
            }

            set
            {
                _loc = value;
                OnPropertyChanged("Loc");
            }
        }

        [JsonProperty(PropertyName = "horario")]
        public virtual DateTime PagoEm
        {
            get
            {
                return _pagoEm;
            }

            set
            {
                _pagoEm = value;
                OnPropertyChanged("PagoEm");
                OnPropertyChanged("PagoEmLocalTime");
            }
        }

        public virtual DateTime PagoEmLocalTime
        {
            get
            {
                return _pagoEm.ToLocalTime();
            }
        }

        public virtual QRCode QrCode
        {
            get
            {
                return _qrCode;
            }

            set
            {
                _qrCode = value;
                OnPropertyChanged("QrCode");
            }
        }

        public virtual Loja Loja
        {
            get
            {
                return _loja;
            }

            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
            }
        }

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual void InicializaLazyLoad()
        {
            if (!NHibernateUtil.IsInitialized(Calendario))
            {
                NHibernateUtil.Initialize(Calendario);
            }

            if (!NHibernateUtil.IsInitialized(Valor))
            {
                NHibernateUtil.Initialize(Valor);
            }

            if (!NHibernateUtil.IsInitialized(Loc))
            {
                NHibernateUtil.Initialize(Loc);
            }

            if (!NHibernateUtil.IsInitialized(QrCode))
            {
                NHibernateUtil.Initialize(QrCode);
            }
        }
    }
}

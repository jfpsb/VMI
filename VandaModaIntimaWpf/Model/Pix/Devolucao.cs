﻿using System;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model
{
    public class Devolucao : AModel, IModel
    {
        private string _id;
        private Pix _pix;
        private Horario _horario;
        private string _rtrId;
        private double _valor;
        private string _status;

        public object GetIdentifier()
        {
            return Id;
        }

        public virtual string Id
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

        public virtual Pix Pix
        {
            get
            {
                return _pix;
            }

            set
            {
                _pix = value;
                OnPropertyChanged("Pix");
            }
        }

        public virtual string RtrId
        {
            get
            {
                return _rtrId;
            }

            set
            {
                _rtrId = value;
                OnPropertyChanged("RtrId");
            }
        }

        public virtual double Valor
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

        public virtual Horario Horario
        {
            get
            {
                return _horario;
            }

            set
            {
                _horario = value;
                OnPropertyChanged("Horario");
            }
        }

        public string GetContextMenuHeader => throw new NotImplementedException();

        public void InicializaLazyLoad()
        {
            throw new NotImplementedException();
        }
    }

    public class Horario : AModel, IModel
    {
        private int _id;
        private DateTime _solicitacao;
        private DateTime _liquidacao;

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

        public virtual DateTime Solicitacao
        {
            get
            {
                return _solicitacao;
            }

            set
            {
                _solicitacao = value;
                OnPropertyChanged("Solicitacao");
            }
        }

        public virtual DateTime Liquidacao
        {
            get
            {
                return _liquidacao;
            }

            set
            {
                _liquidacao = value;
                OnPropertyChanged("Liquidacao");
            }
        }

        public string GetContextMenuHeader => throw new NotImplementedException();

        public object GetIdentifier()
        {
            return Id;
        }

        public void InicializaLazyLoad()
        {
            throw new NotImplementedException();
        }
    }
}

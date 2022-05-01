using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class InscricaoImobiliaria : AModel, IModel
    {
        private int _id;
        private Loja _loja;
        private string _numeracao;
        private DateTime _inicioLocacao;
        private DateTime? _fimLocacao;

        public Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public string GetContextMenuHeader => Numeracao;

        public int Id
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

        public Loja Loja
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

        public string Numeracao
        {
            get
            {
                return _numeracao;
            }

            set
            {
                _numeracao = value;
                OnPropertyChanged("Numeracao");
            }
        }

        public DateTime InicioLocacao
        {
            get
            {
                return _inicioLocacao;
            }

            set
            {
                _inicioLocacao = value;
                OnPropertyChanged("InicioLocacao");
            }
        }

        public DateTime? FimLocacao
        {
            get
            {
                return _fimLocacao;
            }

            set
            {
                _fimLocacao = value;
                OnPropertyChanged("FimLocacao");
            }
        }

        public object GetIdentifier()
        {
            return Id;
        }

        public void InicializaLazyLoad()
        {
            throw new NotImplementedException();
        }

        public bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

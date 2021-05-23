using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Representante : AModel, IModel
    {
        private long _id;
        private string _nome;
        private string _whatsapp;
        private string _cidadeEstado;
        private string _email;
        public Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public string GetContextMenuHeader => throw new NotImplementedException();

        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Nome
        {
            get => _nome;
            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
            }
        }
        public string Whatsapp
        {
            get => _whatsapp;
            set
            {
                _whatsapp = value;
                OnPropertyChanged("Whatsapp");
            }
        }
        public string CidadeEstado
        {
            get => _cidadeEstado;
            set
            {
                _cidadeEstado = value;
                OnPropertyChanged("CidadeEstado");
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        public object GetIdentifier()
        {
            return Id;
        }

        public void InicializaLazyLoad()
        {

        }

        public bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

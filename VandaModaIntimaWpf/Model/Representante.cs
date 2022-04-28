using NHibernate;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Representante : AModel, IModel
    {
        private int _id;
        private string _nome;
        private string _whatsapp;
        private string _cidadeEstado;
        private string _email;
        private IList<Fornecedor> _fornecedores = new List<Fornecedor>();

        public Representante()
        {
        }

        public Representante(string nome)
        {
            Nome = nome;
        }

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => Nome;

        public virtual int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual string Nome
        {
            get => _nome;
            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
            }
        }
        public virtual string Whatsapp
        {
            get => _whatsapp;
            set
            {
                _whatsapp = value;
                OnPropertyChanged("Whatsapp");
            }
        }
        public virtual string CidadeEstado
        {
            get => _cidadeEstado;
            set
            {
                _cidadeEstado = value;
                OnPropertyChanged("CidadeEstado");
            }
        }
        public virtual string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        public virtual IList<Fornecedor> Fornecedores
        {
            get => _fornecedores;
            set
            {
                _fornecedores = value;
                OnPropertyChanged("Fornecedores");
            }
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual void InicializaLazyLoad()
        {
            if (!NHibernateUtil.IsInitialized(Fornecedores))
            {
                NHibernateUtil.Initialize(Fornecedores);
            }
        }

        public virtual bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

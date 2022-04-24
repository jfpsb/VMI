using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class EntradaDeMercadoria : AModel, IModel
    {
        private Guid _id;
        private Loja _loja;
        private DateTime _data;
        private IList<EntradaMercadoriaProdutoGrade> _entradas = new List<EntradaMercadoriaProdutoGrade>();

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => $"{Data.ToLongDateString()} - {Loja.Nome}";

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual void InicializaLazyLoad()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }

        public virtual Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual Loja Loja
        {
            get => _loja;
            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
            }
        }
        public virtual DateTime Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }
        public virtual IList<EntradaMercadoriaProdutoGrade> Entradas
        {
            get => _entradas;
            set
            {
                _entradas = value;
                OnPropertyChanged("Entradas");
            }
        }
    }
}

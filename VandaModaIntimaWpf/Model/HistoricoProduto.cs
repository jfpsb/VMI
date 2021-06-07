using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class HistoricoProduto : AModel, IModel
    {
        private long _id;
        private Produto _produto;
        private DateTime _dataAlteracao;
        private string _descricao;

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual void InicializaLazyLoad()
        {

        }

        public virtual bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }

        public virtual long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual Produto Produto
        {
            get => _produto;
            set
            {
                _produto = value;
                OnPropertyChanged("Produto");
            }
        }
        public virtual DateTime DataAlteracao
        {
            get => _dataAlteracao;
            set
            {
                _dataAlteracao = value;
                OnPropertyChanged("DataAlteracao");
            }
        }
        public virtual string Descricao
        {
            get => _descricao;
            set
            {
                _descricao = value;
                OnPropertyChanged("Descricao");
            }
        }
    }
}

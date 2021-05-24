using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class ArquivosCompraFornecedor : AModel, IModel
    {
        private long _id;
        private CompraDeFornecedor _compraDeFornecedor;
        private string _caminho;
        private string _extensao;
        private string _descricao;

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual CompraDeFornecedor CompraDeFornecedor
        {
            get => _compraDeFornecedor;
            set
            {
                _compraDeFornecedor = value;
                OnPropertyChanged("CompraDeFornecedor");
            }
        }
        public virtual string Caminho
        {
            get => _caminho;
            set
            {
                _caminho = value;
                OnPropertyChanged("Caminho");
            }
        }
        public virtual string Extensao
        {
            get => _extensao;
            set
            {
                _extensao = value;
                OnPropertyChanged("Extensao");
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
    }
}

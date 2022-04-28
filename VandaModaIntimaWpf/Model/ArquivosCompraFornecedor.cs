using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class ArquivosCompraFornecedor : AModel, IModel
    {
        private int _id;
        private CompraDeFornecedor _compraDeFornecedor;
        private string _nome;
        private string _extensao;
        private string _caminhoOriginal;

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual int Id
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
        public virtual string Nome
        {
            get => _nome;
            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
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

        public virtual string CaminhoOriginal { get => _caminhoOriginal; set => _caminhoOriginal = value; }

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

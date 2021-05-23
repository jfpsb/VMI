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
        public CompraDeFornecedor CompraDeFornecedor
        {
            get => _compraDeFornecedor;
            set
            {
                _compraDeFornecedor = value;
                OnPropertyChanged("CompraDeFornecedor");
            }
        }
        public string Caminho
        {
            get => _caminho;
            set
            {
                _caminho = value;
                OnPropertyChanged("Caminho");
            }
        }
        public string Extensao
        {
            get => _extensao;
            set
            {
                _extensao = value;
                OnPropertyChanged("Extensao");
            }
        }
        public string Descricao
        {
            get => _descricao;
            set
            {
                _descricao = value;
                OnPropertyChanged("Descricao");
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

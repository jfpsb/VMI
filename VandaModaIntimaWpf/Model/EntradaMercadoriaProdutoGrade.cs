using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class EntradaMercadoriaProdutoGrade : AModel, IModel
    {
        private long _id;
        private EntradaDeMercadoria _entrada;
        private ProdutoGrade _produtoGrade;
        private int _quantidade;

        public enum Colunas
        {
            CodBarra = 1,
            CodBarraAlt = 2,
            Descricao = 3,
            Grade = 4,
            Preco = 5,
            Quantidade = 6
        }

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => ProdutoGrade.SubGradesToString;

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

        public virtual long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual EntradaDeMercadoria Entrada
        {
            get => _entrada;
            set
            {
                _entrada = value;
                OnPropertyChanged("Entrada");
            }
        }
        public virtual ProdutoGrade ProdutoGrade
        {
            get => _produtoGrade;
            set
            {
                _produtoGrade = value;
                OnPropertyChanged("ProdutoGrade");
            }
        }
        public virtual int Quantidade
        {
            get => _quantidade;
            set
            {
                _quantidade = value;
                OnPropertyChanged("Quantidade");
            }
        }
    }
}

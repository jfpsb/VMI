using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Estoque : AModel, IModel
    {
        private ProdutoGrade _produtoGrade;
        private Loja _loja;
        private int _quantidade;

        public virtual ProdutoGrade ProdutoGrade
        {
            get
            {
                return _produtoGrade;
            }

            set
            {
                _produtoGrade = value;
                OnPropertyChanged("ProdutoGrade");
            }
        }

        public virtual Loja Loja
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

        public virtual int Quantidade
        {
            get
            {
                return _quantidade;
            }

            set
            {
                _quantidade = value;
                OnPropertyChanged("Quantidade");
            }
        }

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => $"{ProdutoGrade.Produto.Descricao}";

        public object GetIdentifier()
        {
            Estoque estoque = new Estoque()
            {
                ProdutoGrade = ProdutoGrade,
                Loja = Loja
            };

            return estoque;
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

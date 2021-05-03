using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Estoque : AModel, IModel
    {
        private ProdutoGrade _produtoGrade;
        private Loja _loja;
        private int _quantidade;

        public ProdutoGrade ProdutoGrade
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

        public int Quantidade
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

        public Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public string GetContextMenuHeader => $"{ProdutoGrade.Produto.Descricao}";

        public object GetIdentifier()
        {
            Estoque estoque = new Estoque()
            {
                ProdutoGrade = ProdutoGrade,
                Loja = Loja
            };

            return estoque;
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

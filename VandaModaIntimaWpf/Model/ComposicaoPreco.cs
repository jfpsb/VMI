using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class ComposicaoPreco : AModel, IModel
    {
        private long _id;
        private Loja _loja;
        private ProdutoGrade _produtoGrade;
        private DateTime _data;
        private double _precoCompra;
        private double _frete;

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
        public virtual Loja Loja
        {
            get => _loja;
            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
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
        public virtual DateTime Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }
        public virtual double ValorSimples
        {
            get => ProdutoGrade.Preco * Loja.UltimaAliquota.Simples;
        }
        public virtual double ValorIcms
        {
            get => PrecoCompra * Loja.UltimaAliquota.Icms;
        }
        public virtual double MargemContribuicao
        {
            get => ProdutoGrade.Preco - CustoTotal;
        }
        public virtual double CustoTotal
        {
            get => PrecoCompra + ValorIcms + ValorSimples + Frete;
        }
        public virtual double PrecoCompra
        {
            get => _precoCompra;
            set
            {
                _precoCompra = value;
                OnPropertyChanged("PrecoCompra");
                OnPropertyChanged("CustoTotal");
                OnPropertyChanged("Lucro");
                OnPropertyChanged("MargemContribuicao");
            }
        }
        public virtual double Frete
        {
            get => _frete;
            set
            {
                _frete = value;
                OnPropertyChanged("Frete");
                OnPropertyChanged("CustoTotal");
                OnPropertyChanged("Lucro");
                OnPropertyChanged("MargemContribuicao");
            }
        }
        public virtual double Lucro
        {
            get => MargemContribuicao / ProdutoGrade.Preco;
        }
    }
}

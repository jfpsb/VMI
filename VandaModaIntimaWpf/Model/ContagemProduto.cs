using System;

namespace VandaModaIntimaWpf.Model
{
    public class ContagemProduto : ObservableObject, ICloneable, IModel
    {
        private long _id;
        private Contagem _contagem;
        private Produto _produto;
        private int _quant;

        public bool IsIdentical(object obj)
        {
            if (obj != null && obj.GetType() == typeof(ContagemProduto))
            {
                ContagemProduto contagemProduto = (ContagemProduto)obj;

                return contagemProduto.Id.Equals(Id)
                       && contagemProduto.Contagem.Equals(Contagem)
                       && contagemProduto.Produto.Equals(Produto)
                       && contagemProduto.Quant.Equals(Quant);
            }

            return false;
        }

        public virtual string GetContextMenuHeader => $"{Produto.CodBarra}; Quantidade: {Quant}";

        public virtual long Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public virtual Produto Produto
        {
            get
            {
                return _produto;
            }

            set
            {
                _produto = value;
                OnPropertyChanged("Produto");
            }
        }

        public virtual int Quant
        {
            get
            {
                return _quant;
            }

            set
            {
                _quant = value;
                OnPropertyChanged("Quant");
            }
        }

        public virtual Contagem Contagem
        {
            get
            {
                return _contagem;
            }

            set
            {
                _contagem = value;
                OnPropertyChanged("Contagem");
            }
        }

        public virtual object Clone()
        {
            throw new NotImplementedException();
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}

using System;

namespace SincronizacaoVMI.Model
{
    public class ContagemProduto : AModel, IModel
    {
        private int _id;
        private Contagem _contagem;
        private ProdutoGrade _produtoGrade;
        private int _quant;

        public virtual int Id
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

        public virtual void Copiar(object source)
        {
            throw new NotImplementedException();
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}

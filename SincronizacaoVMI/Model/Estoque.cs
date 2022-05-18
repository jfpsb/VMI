using System;
using System.Collections.Generic;

namespace SincronizacaoVMI.Model
{
    public class Estoque : AModel, IModel
    {
        private int _id;
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

        public int Id
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
        public object GetIdentifier()
        {
            return Id;
        }
    }
}

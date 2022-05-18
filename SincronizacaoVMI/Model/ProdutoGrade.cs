using System;
using System.Collections.Generic;

namespace SincronizacaoVMI.Model
{
    public class ProdutoGrade : AModel, IModel
    {
        private int _id;
        private string _codBarra;
        private string _codBarraAlternativo;
        private Produto _produto;
        private double _preco;
        private double _precoCusto;
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
        public virtual string CodBarra
        {
            get
            {
                return _codBarra;
            }

            set
            {
                _codBarra = value;
                OnPropertyChanged("CodBarra");
            }
        }

        public virtual double Preco
        {
            get
            {
                return _preco;
            }

            set
            {
                _preco = value;
                OnPropertyChanged("Preco");
            }
        }

        public virtual double PrecoCusto
        {
            get => _precoCusto;
            set
            {
                _precoCusto = value;
                OnPropertyChanged("PrecoCusto");
                OnPropertyChanged("MargemDeLucro");
            }
        }

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
        public virtual string CodBarraAlternativo
        {
            get => _codBarraAlternativo;
            set
            {
                _codBarraAlternativo = value;
                OnPropertyChanged("CodBarraAlternativo");
            }
        }
        public virtual object GetIdentifier()
        {
            return this;
        }
    }
}

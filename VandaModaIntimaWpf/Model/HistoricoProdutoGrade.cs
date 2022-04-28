using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class HistoricoProdutoGrade : AModel, IModel
    {
        private int _id;
        private ProdutoGrade _produtoGrade;
        private DateTime _data;
        private double _precoCompra;
        private double _precoVenda;
        private double _custoTotal;
        private double _frete;

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
        public virtual ProdutoGrade ProdutoGrade { get => _produtoGrade; set { _produtoGrade = value; OnPropertyChanged("ProdutoGrade"); } }
        public virtual DateTime Data { get => _data; set { _data = value; OnPropertyChanged("Data"); } }
        public virtual double PrecoCompra { get => _precoCompra; set { _precoCompra = value; OnPropertyChanged("PrecoCompra"); } }
        public virtual double PrecoVenda { get => _precoVenda; set { _precoVenda = value; OnPropertyChanged("PrecoVenda"); } }
        public virtual double CustoTotal { get => _custoTotal; set { _custoTotal = value; OnPropertyChanged("CustoTotal"); } }
        public virtual double Frete { get => _frete; set { _frete = value; OnPropertyChanged("Frete"); } }
        //public virtual double Lucro { get => _lucro; set { _lucro = value; OnPropertyChanged("Lucro"); } }

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual void InicializaLazyLoad()
        {

        }

        public virtual bool IsIdentical(object obj)
        {
            return false;
        }
    }
}

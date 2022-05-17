using System;

namespace SincronizacaoVMI.Model
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

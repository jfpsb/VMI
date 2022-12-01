using System;

namespace SincronizacaoVMI.Model
{
    public class ParcelaCartao : AModel, IModel
    {
        private int _id;
        private VendaEmCartao _vendaEmCartao;
        private DateTime _dataPagamento;
        private double _valorBruto;
        private double _valorLiquido;

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

        public virtual VendaEmCartao VendaEmCartao
        {
            get
            {
                return _vendaEmCartao;
            }

            set
            {
                _vendaEmCartao = value;
                OnPropertyChanged("VendaEmCartao");
            }
        }

        public virtual DateTime DataPagamento
        {
            get
            {
                return _dataPagamento;
            }

            set
            {
                _dataPagamento = value;
                OnPropertyChanged("DataPagamento");
            }
        }

        public virtual double ValorBruto
        {
            get
            {
                return _valorBruto;
            }

            set
            {
                _valorBruto = value;
                OnPropertyChanged("ValorBruto");
            }
        }

        public virtual double ValorLiquido
        {
            get
            {
                return _valorLiquido;
            }

            set
            {
                _valorLiquido = value;
                OnPropertyChanged("ValorLiquido");
            }
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}

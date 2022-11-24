using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class VendaEmCartao : AModel, IModel
    {
        private int _id;
        private OperadoraCartao _operadoraCartao;
        private Loja _loja;
        private DateTime _dataHora;
        private double _valorBruto;
        private double _valorLiquido;
        private string _modalidade;
        private string _bandeira;
        private string _nsuRede;
        private string _rvCredishop;
        private string _numPedidoCaixa;
        private IList<ParcelaCartao> _parcelas = new List<ParcelaCartao>();

        public virtual string GetContextMenuHeader => $"{DataHora:dd/MM/yyyy HH:mm} - BRUTO: {ValorBruto:C} - {ValorLiquido:C} - {OperadoraCartao.Nome}";

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

        public virtual OperadoraCartao OperadoraCartao
        {
            get
            {
                return _operadoraCartao;
            }

            set
            {
                _operadoraCartao = value;
                OnPropertyChanged("OperadoraCartao");
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

        public virtual DateTime DataHora
        {
            get
            {
                return _dataHora;
            }

            set
            {
                _dataHora = value;
                OnPropertyChanged("DataHora");
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

        public virtual string Modalidade
        {
            get
            {
                return _modalidade;
            }

            set
            {
                _modalidade = value;
                OnPropertyChanged("Modalidade");
            }
        }

        public virtual string Bandeira
        {
            get
            {
                return _bandeira;
            }

            set
            {
                _bandeira = value;
                OnPropertyChanged("Bandeira");
            }
        }

        public virtual string NsuRede
        {
            get
            {
                return _nsuRede;
            }

            set
            {
                _nsuRede = value;
                OnPropertyChanged("NsuRede");
            }
        }

        public virtual string RvCredishop
        {
            get
            {
                return _rvCredishop;
            }

            set
            {
                _rvCredishop = value;
                OnPropertyChanged("RvCredishop");
            }
        }

        public virtual string NumPedidoCaixa
        {
            get
            {
                return _numPedidoCaixa;
            }

            set
            {
                _numPedidoCaixa = value;
                OnPropertyChanged("NumPedidoCaixa");
            }
        }

        public virtual IList<ParcelaCartao> Parcelas
        {
            get
            {
                return _parcelas;
            }

            set
            {
                _parcelas = value;
                OnPropertyChanged("Parcelas");
            }
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual void InicializaLazyLoad()
        {
            throw new NotImplementedException();
        }
    }
}

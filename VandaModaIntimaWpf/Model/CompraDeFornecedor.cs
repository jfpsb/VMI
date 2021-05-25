using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class CompraDeFornecedor : AModel, IModel
    {
        private long _id;
        private Fornecedor _fornecedor;
        private Loja _loja;
        private IList<ArquivosCompraFornecedor> _arquivos = new List<ArquivosCompraFornecedor>();
        DateTime _dataPedido;
        DateTime _dataNotaFiscal;
        private int _numeroNfe;
        private string _chaveAcessoNfe;
        bool _pago;
        double _valor;

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual Fornecedor Fornecedor
        {
            get => _fornecedor;
            set
            {
                _fornecedor = value;
                OnPropertyChanged("Fornecedor");
            }
        }
        public virtual DateTime DataPedido
        {
            get => _dataPedido;
            set
            {
                _dataPedido = value;
                OnPropertyChanged("DataPedido");
            }
        }
        public virtual DateTime DataNotaFiscal
        {
            get => _dataNotaFiscal;
            set
            {
                _dataNotaFiscal = value;
                OnPropertyChanged("DataNotaFiscal");
            }
        }
        public virtual bool Pago
        {
            get => _pago;
            set
            {
                _pago = value;
                OnPropertyChanged("Pago");
            }
        }
        public virtual double Valor
        {
            get => _valor;
            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }

        public virtual IList<ArquivosCompraFornecedor> Arquivos
        {
            get => _arquivos;
            set
            {
                _arquivos = value;
                OnPropertyChanged("Arquivos");
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

        public virtual int NumeroNfe
        {
            get => _numeroNfe;
            set
            {
                _numeroNfe = value;
                OnPropertyChanged("NumeroNfe");
            }
        }
        public virtual string ChaveAcessoNfe
        {
            get => _chaveAcessoNfe;
            set
            {
                _chaveAcessoNfe = value;
                OnPropertyChanged("ChaveAcessoNfe");
            }
        }

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

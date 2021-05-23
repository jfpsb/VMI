using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class CompraDeFornecedor : AModel, IModel
    {
        private long _id;
        private Fornecedor _fornecedor;
        private Loja _loja;
        private IList<ArquivosCompraFornecedor> _arquivos;
        DateTime _dataPedido;
        DateTime _dataNotaFiscal;
        bool _pago;
        double _valor;

        public Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public string GetContextMenuHeader => throw new NotImplementedException();

        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public Fornecedor Fornecedor
        {
            get => _fornecedor;
            set
            {
                _fornecedor = value;
                OnPropertyChanged("Fornecedor");
            }
        }
        public DateTime DataPedido
        {
            get => _dataPedido;
            set
            {
                _dataPedido = value;
                OnPropertyChanged("DataPedido");
            }
        }
        public DateTime DataNotaFiscal
        {
            get => _dataNotaFiscal;
            set
            {
                _dataNotaFiscal = value;
                OnPropertyChanged("DataNotaFiscal");
            }
        }
        public bool Pago
        {
            get => _pago;
            set
            {
                _pago = value;
                OnPropertyChanged("Pago");
            }
        }
        public double Valor
        {
            get => _valor;
            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }

        public IList<ArquivosCompraFornecedor> Arquivos
        {
            get => _arquivos;
            set
            {
                _arquivos = value;
                OnPropertyChanged("Arquivos");
            }
        }

        public Loja Loja
        {
            get => _loja;
            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
            }
        }

        public object GetIdentifier()
        {
            return Id;
        }

        public void InicializaLazyLoad()
        {

        }

        public bool IsIdentical(object obj)
        {
            return false;
        }
    }
}

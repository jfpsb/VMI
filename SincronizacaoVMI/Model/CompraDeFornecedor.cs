﻿using System;

namespace SincronizacaoVMI.Model
{
    public class CompraDeFornecedor : AModel, IModel
    {
        private int _id;
        private Fornecedor _fornecedor;
        private Representante _representante;
        private Loja _loja;
        DateTime _dataPedido;
        DateTime? _dataNotaFiscal;
        private int _numeroNfe;
        private string _chaveAcessoNfe;
        bool _pago;
        double _valor;
        public virtual int Id
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
        public virtual DateTime? DataNotaFiscal
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

        public virtual Representante Representante
        {
            get => _representante;
            set
            {
                _representante = value;
                OnPropertyChanged("Representante");
            }
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}

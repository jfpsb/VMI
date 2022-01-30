using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Despesa : AModel, IModel
    {
        private long _id;
        private TipoDespesa _tipoDespesa;
        private Adiantamento _adiantamento;
        private Fornecedor _fornecedor;
        private Representante _representante;
        private Loja _loja;
        private DateTime _data;
        private DateTime? _dataVencimento;
        private string _descricao;
        private double _valor;
        private string _familiar;

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => $"{TipoDespesa.Nome} - {Valor}";

        public virtual long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual TipoDespesa TipoDespesa
        {
            get => _tipoDespesa;
            set
            {
                _tipoDespesa = value;
                OnPropertyChanged("TipoDespesa");
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
        public virtual DateTime Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }
        public virtual string Descricao
        {
            get => _descricao;
            set
            {
                if (value != null)
                {
                    _descricao = value.ToUpper(); ;
                    OnPropertyChanged("Descricao");
                }
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
        public virtual string Familiar
        {
            get => _familiar;
            set
            {
                _familiar = value;
                OnPropertyChanged("Familiar");
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

        public virtual Loja Loja
        {
            get => _loja;
            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
            }
        }

        public virtual DateTime? DataVencimento
        {
            get => _dataVencimento;
            set
            {
                _dataVencimento = value;
                OnPropertyChanged("DataVencimento");
            }
        }

        public virtual Adiantamento Adiantamento
        {
            get => _adiantamento;
            set
            {
                _adiantamento = value;
                OnPropertyChanged("Adiantamento");
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
            throw new NotImplementedException();
        }
    }
}

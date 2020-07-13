using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Adiantamento : ObservableObject, ICloneable, IModel
    {
        private DateTime _data;
        private FolhaPagamento _folhaPagamento;
        private double _valor;
        private IList<Parcela> _parcelas = new List<Parcela>();
        public string GetContextMenuHeader => throw new NotImplementedException();

        public DateTime Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }
        public FolhaPagamento FolhaPagamento
        {
            get => _folhaPagamento;
            set
            {
                _folhaPagamento = value;
                OnPropertyChanged("FolhaPagamento");
            }
        }
        public IList<Parcela> Parcelas
        {
            get => _parcelas;
            set
            {
                _parcelas = value;
                OnPropertyChanged("Parcelas");
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

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public object GetIdentifier()
        {
            return _data;
        }

        public bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

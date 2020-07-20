using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class FolhaPagamento : ObservableObject, ICloneable, IModel
    {
        private int _id;
        private int _mes;
        private int _ano;
        private Funcionario _funcionario;
        private double _valor;
        private double _valorAPagar;
        private bool _fechada;
        private IList<Parcela> _parcelas = new List<Parcela>();

        public string GetContextMenuHeader => _mes + "/" + _ano + " - " + _funcionario.Nome;

        public int Mes
        {
            get => _mes;
            set
            {
                _mes = value;
                OnPropertyChanged("Mes");
            }
        }
        public int Ano
        {
            get => _ano;
            set
            {
                _ano = value;
                OnPropertyChanged("Ano");
            }
        }
        public string MesReferencia
        {
            get => _mes + "/" + _ano;
        }
        public Funcionario Funcionario
        {
            get => _funcionario;
            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
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
        public bool Fechada
        {
            get => _fechada;
            set
            {
                _fechada = value;
                OnPropertyChanged("Fechada");
            }
        }
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
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

        public double ValorAPagar
        {
            get
            {
                _valorAPagar = _funcionario.Salario;

                foreach (Parcela parcela in _parcelas)
                {
                    _valorAPagar -= parcela.Valor;
                }

                return _valorAPagar;
            }
            set
            {
                _valorAPagar = value;
                OnPropertyChanged("ValorAPagar");
            }
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
        public object GetIdentifier()
        {
            return _id;
        }
        public bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

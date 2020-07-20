using System;

namespace VandaModaIntimaWpf.Model
{
    public class Parcela : ObservableObject, ICloneable, IModel
    {
        private long _id;
        private Adiantamento _adiantamento;
        private FolhaPagamento _folhapagamento;
        private int _numero;
        private double _valor;
        private bool _paga;

        public string GetContextMenuHeader => throw new NotImplementedException();

        public Adiantamento Adiantamento
        {
            get => _adiantamento;
            set
            {
                _adiantamento = value;
                OnPropertyChanged("Adiantamento");
            }
        }
        public FolhaPagamento FolhaPagamento
        {
            get => _folhapagamento;
            set
            {
                _folhapagamento = value;
                OnPropertyChanged("FolhaPagamento");
            }
        }
        public string PagamentoEm
        {
            get => FolhaPagamento.Mes + "/" + FolhaPagamento.Ano;
        }
        public double Valor
        {
            get => Math.Round(_valor, 2);
            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }
        public bool Paga
        {
            get => _paga;
            set
            {
                _paga = value;
                OnPropertyChanged("Paga");
            }
        }

        public int Numero
        {
            get => _numero;
            set
            {
                _numero = value;
                OnPropertyChanged("Numero");
            }
        }
        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        /// <summary>
        /// Retorna valor acumulado de parcelas sendo cobradas no mesmo mês que esta parcela
        /// </summary>
        public double ValorAcumulado
        {
            get
            {
                double valor = Valor;

                foreach (Parcela p in FolhaPagamento.Parcelas)
                {
                    valor += p.Valor;
                }

                return FolhaPagamento.Funcionario.Salario - valor;
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

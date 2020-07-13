using System;

namespace VandaModaIntimaWpf.Model
{
    public class Parcela : ObservableObject, ICloneable, IModel
    {
        private int _mes;
        private int _ano;
        private Adiantamento _adiantamento;
        private double _valor;
        private bool _paga;

        public string GetContextMenuHeader => throw new NotImplementedException();

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
        public Adiantamento Adiantamento
        {
            get => _adiantamento;
            set
            {
                _adiantamento = value;
                OnPropertyChanged("Adiantamento");
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
        public bool Paga
        {
            get => _paga;
            set
            {
                _paga = value;
                OnPropertyChanged("Paga");
            }
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public object GetIdentifier()
        {
            return this;
        }

        public bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Parcela))
            {
                Parcela parcela = (Parcela)obj;

                if (parcela.Mes == Mes && parcela.Ano == Ano && parcela.Adiantamento.Equals(Adiantamento))
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Mes.GetHashCode() + Ano.GetHashCode() + Adiantamento.GetHashCode();
        }
    }
}

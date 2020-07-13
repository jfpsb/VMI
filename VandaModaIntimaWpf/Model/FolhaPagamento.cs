using System;

namespace VandaModaIntimaWpf.Model
{
    public class FolhaPagamento : ObservableObject, ICloneable, IModel
    {
        private int _mes;
        private int _ano;
        private Funcionario _funcionario;
        private double _valor;
        private bool _fechada;

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
            if (obj.GetType() == typeof(FolhaPagamento))
            {
                FolhaPagamento folhaPagamento = obj as FolhaPagamento;

                if (folhaPagamento.Mes == Mes && folhaPagamento.Ano == Ano && folhaPagamento.Funcionario.Equals(Funcionario))
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Mes.GetHashCode() + Ano.GetHashCode() + Funcionario.GetHashCode();
        }
    }
}

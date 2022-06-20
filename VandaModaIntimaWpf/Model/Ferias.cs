using System;

namespace VandaModaIntimaWpf.Model
{
    public class Ferias : AModel, IModel
    {
        private int _id;
        private Funcionario _funcionario;
        private DateTime _inicio;
        private DateTime _fim;
        private DateTime _inicioAquisitivo;

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

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

        public virtual Funcionario Funcionario
        {
            get
            {
                return _funcionario;
            }

            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
            }
        }

        public virtual DateTime Inicio
        {
            get
            {
                return _inicio;
            }

            set
            {
                _inicio = value;
                OnPropertyChanged("Inicio");
            }
        }

        public virtual DateTime InicioAquisitivo
        {
            get
            {
                return _inicioAquisitivo;
            }

            set
            {
                _inicioAquisitivo = value;
                OnPropertyChanged("InicioAquisitivo");
            }
        }

        public virtual DateTime FimAquisitivo
        {
            get => InicioAquisitivo.AddYears(1).AddDays(-1);
        }

        public virtual DateTime InicioConcessivo
        {
            get => FimAquisitivo.AddDays(1);
        }

        public virtual DateTime FimConcessivo
        {
            get => InicioConcessivo.AddYears(1).AddDays(-1);
        }

        public virtual DateTime Fim
        {
            get
            {
                return _fim;
            }

            set
            {
                _fim = value;
                OnPropertyChanged("Fim");
            }
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual void InicializaLazyLoad()
        {

        }
    }
}

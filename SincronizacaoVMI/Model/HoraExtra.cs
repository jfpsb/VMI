using System;

namespace SincronizacaoVMI.Model
{
    public class HoraExtra : AModel, IModel
    {
        private int _id;
        private Funcionario _funcionario;
        private TipoHoraExtra _tipoHoraExtra;
        private Loja _lojaTrabalho;
        private DateTime _data;
        private int _horas;
        private int _minutos;

        public virtual int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual Funcionario Funcionario
        {
            get => _funcionario;
            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
            }
        }
        public virtual TipoHoraExtra TipoHoraExtra
        {
            get => _tipoHoraExtra;
            set
            {
                _tipoHoraExtra = value;
                OnPropertyChanged("TipoHoraExtra");
            }
        }
        public virtual Loja LojaTrabalho
        {
            get => _lojaTrabalho;
            set
            {
                _lojaTrabalho = value;
                OnPropertyChanged("LojaTrabalho");
            }
        }
        public virtual DateTime Data
        {
            get
            {
                return _data;
            }

            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }
        public virtual int Horas
        {
            get => _horas;
            set
            {
                _horas = value;
                OnPropertyChanged("Horas");
            }
        }
        public virtual int Minutos
        {
            get => _minutos;
            set
            {
                _minutos = value;
                OnPropertyChanged("Minutos");
            }
        }

        public virtual string TotalEmString
        {
            get
            {
                if (Horas == 0 && Minutos == 0)
                {
                    return "-- : --";
                }
                else
                {
                    return string.Format("{0:0#}:{1:0#}", Horas, Minutos);
                }
            }
        }

        public virtual TimeSpan EmTimeSpan
        {
            get
            {
                TimeSpan timeSpan = new TimeSpan(Horas, Minutos, 0);
                return timeSpan;
            }
        }
        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}

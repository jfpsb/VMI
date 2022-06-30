using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
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

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

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
                    int horas = Horas;
                    int minutos = Minutos;

                    if (Minutos >= 60)
                    {
                        int minutosParaHoras = Minutos / 60;
                        horas += minutosParaHoras; //Soma às horas os minutos convertidos em horas
                        minutos = Minutos % 60; //Restante dos minutos após converter em horas
                    }

                    return string.Format("{0:0#}:{1:0#}", horas, minutos);
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

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual void InicializaLazyLoad()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

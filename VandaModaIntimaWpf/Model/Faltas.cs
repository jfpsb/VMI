using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Faltas : AModel, IModel
    {
        private int _id;
        private Funcionario _funcionario;
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
        public virtual DateTime Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }
        public virtual TimeSpan EmTimeSpan
        {
            get => new TimeSpan(Horas, Minutos, 0);
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

using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class HoraExtra : AModel, IModel
    {
        private long _id;
        private Funcionario _funcionario;
        private TipoHoraExtra _tipoHoraExtra;
        private int _mes;
        private int _ano;
        private int _horas;
        private int _minutos;

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual long Id
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
        public virtual int Mes
        {
            get => _mes;
            set
            {
                _mes = value;
                OnPropertyChanged("Mes");
            }
        }
        public virtual int Ano
        {
            get => _ano;
            set
            {
                _ano = value;
                OnPropertyChanged("Ano");
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

        public virtual string HorasEmString
        {
            get
            {
                TimeSpan timeSpan = new TimeSpan(Horas, Minutos, 0);
                return timeSpan.ToString(@"hh\:mm");
            }
        }

        public virtual TimeSpan HorasTimeSpan
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

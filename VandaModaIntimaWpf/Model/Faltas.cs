﻿using System;

namespace VandaModaIntimaWpf.Model
{
    public class Faltas : AModel, IModel
    {
        private int _id;
        private Funcionario _funcionario;
        private DateTime _data;
        private int _horas;
        private int _minutos;
        private bool _justificado;
        private string _justificativa;

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

        public virtual bool Justificado
        {
            get
            {
                return _justificado;
            }

            set
            {
                _justificado = value;
                OnPropertyChanged("Justificado");
            }
        }

        public virtual string Justificativa
        {
            get
            {
                return _justificativa;
            }

            set
            {
                if (value != null)
                {
                    _justificativa = value.ToUpper();
                    OnPropertyChanged("Justificativa");
                }
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

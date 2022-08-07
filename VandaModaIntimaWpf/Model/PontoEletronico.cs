using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class PontoEletronico : AModel, IModel
    {
        private int _id;
        private Funcionario _funcionario;
        private DateTime _dia;
        private DateTime? _entrada;
        private DateTime? _saida;
        private IList<IntervaloPonto> _intervalos = new List<IntervaloPonto>();
        private bool _isDiaUtil; //Guarda se o dia do ponto eletrônico será considerado um dia útil caso seja um feriado com dia de trabalho normal
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

        public virtual DateTime? Entrada
        {
            get
            {
                return _entrada;
            }

            set
            {
                _entrada = value;
                OnPropertyChanged("Entrada");
            }
        }

        public virtual DateTime? Saida
        {
            get
            {
                return _saida;
            }

            set
            {
                _saida = value;
                OnPropertyChanged("Saida");
            }
        }
        public virtual DateTime Dia
        {
            get
            {
                return _dia;
            }

            set
            {
                _dia = value;
                OnPropertyChanged("Dia");
            }
        }

        public virtual TimeSpan Jornada
        {
            get
            {
                if (Saida != null && Entrada != null)
                {
                    TimeSpan totalNoDia = DateTime.Parse(Saida.Value.ToShortTimeString()).Subtract(DateTime.Parse(Entrada.Value.ToShortTimeString()));
                    TimeSpan intervaloTotal = new TimeSpan();
                    foreach (var intervalo in Intervalos)
                    {
                        if (intervalo.Fim == null) continue;
                        TimeSpan ts = DateTime.Parse(intervalo.Fim.Value.ToShortTimeString()).Subtract(DateTime.Parse(intervalo.Inicio.ToShortTimeString()));
                        intervaloTotal = intervaloTotal.Add(ts);
                    }
                    return totalNoDia - intervaloTotal;
                }

                return new TimeSpan();
            }
        }

        public virtual string IntervalosEmString
        {
            get
            {
                if (Intervalos.Count == 0)
                    return "SEM INTERVALOS";

                string result = "";
                foreach (var i in Intervalos)
                {
                    var fim = i.Fim == null ? "-- : --" : i.Fim.Value.ToString("HH:mm");
                    result += $"{i.Inicio:HH:mm} a {fim}\n";
                }
                return result.Remove(result.Length - 1);
            }
        }

        public virtual IList<IntervaloPonto> Intervalos
        {
            get
            {
                return _intervalos;
            }

            set
            {
                _intervalos = value;
                OnPropertyChanged("Intervalos");
            }
        }

        public virtual bool IsDiaUtil
        {
            get
            {
                return _isDiaUtil;
            }

            set
            {
                _isDiaUtil = value;
                OnPropertyChanged("IsDiaUtil");
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

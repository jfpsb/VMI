using System;

namespace VandaModaIntimaWpf.Model
{
    public class PontoEletronico : AModel, IModel
    {
        private int _id;
        private Funcionario _funcionario;
        private DateTime _dia;
        private DateTime? _entrada;
        private DateTime? _saida;
        private DateTime? _entradaAlmoco;
        private DateTime? _saidaAlmoco;
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

        /// <summary>
        /// Horário de ínício de almoço/descanso.
        /// </summary>
        public virtual DateTime? EntradaAlmoco
        {
            get
            {
                return _entradaAlmoco;
            }

            set
            {
                _entradaAlmoco = value;
                OnPropertyChanged("EntradaAlmoco");
            }
        }

        /// <summary>
        /// Horário de fim de almoço/descanso.
        /// </summary>
        public virtual DateTime? SaidaAlmoco
        {
            get
            {
                return _saidaAlmoco;
            }

            set
            {
                _saidaAlmoco = value;
                OnPropertyChanged("SaidaAlmoco");
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
                    TimeSpan descanso = DateTime.Parse(SaidaAlmoco.Value.ToShortTimeString()).Subtract(DateTime.Parse(EntradaAlmoco.Value.ToShortTimeString()));
                    return totalNoDia - descanso;
                }

                return new TimeSpan();
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

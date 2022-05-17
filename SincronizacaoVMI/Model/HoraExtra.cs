using System;

namespace SincronizacaoVMI.Model
{
    public class HoraExtra : AModel, IModel
    {
        private int _id;
        private Funcionario _funcionario;
        private TipoHoraExtra _tipoHoraExtra;
        private Loja _lojaTrabalho;
        private int _mes;
        private int _ano;
        private int _horas;
        private int _minutos;
        private string _descricao;

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
                Descricao = TipoHoraExtra.Descricao;
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

        public virtual string Descricao
        {
            get
            {
                return _descricao;
            }

            set
            {
                _descricao = value;
                OnPropertyChanged("Descricao");
            }
        }

        public virtual void Copiar(object source)
        {
            throw new NotImplementedException();
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}

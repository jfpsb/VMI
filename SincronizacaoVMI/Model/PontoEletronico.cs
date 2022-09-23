using System;

namespace SincronizacaoVMI.Model
{
    public class PontoEletronico : AModel, IModel
    {
        private int _id;
        private Funcionario _funcionario;
        private DateTime _dia;
        private DateTime? _entrada;
        private DateTime? _saida;
        private bool _isDiaUtil; //Guarda se o dia do ponto eletrônico será considerado um dia útil caso seja um feriado com dia de trabalho normal

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
    }
}

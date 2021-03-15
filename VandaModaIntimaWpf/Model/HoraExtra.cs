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

        public Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public string GetContextMenuHeader => throw new NotImplementedException();

        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public Funcionario Funcionario
        {
            get => _funcionario;
            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
            }
        }
        public TipoHoraExtra TipoHoraExtra
        {
            get => _tipoHoraExtra;
            set
            {
                _tipoHoraExtra = value;
                OnPropertyChanged("TipoHoraExtra");
            }
        }
        public int Mes
        {
            get => _mes;
            set
            {
                _mes = value;
                OnPropertyChanged("Mes");
            }
        }
        public int Ano
        {
            get => _ano;
            set
            {
                _ano = value;
                OnPropertyChanged("Ano");
            }
        }
        public int Horas
        {
            get => _horas;
            set
            {
                _horas = value;
                OnPropertyChanged("Horas");
            }
        }
        public int Minutos
        {
            get => _minutos;
            set
            {
                _minutos = value;
                OnPropertyChanged("Minutos");
            }
        }

        public string CouchDbId()
        {
            return Id.ToString();
        }

        public object GetIdentifier()
        {
            return Id;
        }

        public void InicializaLazyLoad()
        {
            throw new NotImplementedException();
        }

        public bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

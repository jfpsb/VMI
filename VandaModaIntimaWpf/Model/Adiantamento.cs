﻿using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Adiantamento : ObservableObject, ICloneable, IModel
    {
        private long _id;
        private Funcionario _funcionario;
        private DateTime _data;
        private double _valor;
        private IList<Parcela> _parcelas = new List<Parcela>();

        public string GetContextMenuHeader => _data.ToString("d") + " - " + _funcionario.Nome;

        public DateTime Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }
        public string DataString
        {
            get => _data.ToString("G");
        }
        public double Valor
        {
            get => _valor;
            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }

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

        public IList<Parcela> Parcelas
        {
            get => _parcelas;
            set { _parcelas = value;
                OnPropertyChanged("Parcelas");
            }
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public object GetIdentifier()
        {
            return _id;
        }

        public bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
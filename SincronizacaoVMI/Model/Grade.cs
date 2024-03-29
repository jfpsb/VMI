﻿using System;

namespace SincronizacaoVMI.Model
{
    public class Grade : AModel, IModel
    {
        private int _id;
        private TipoGrade _tipoGrade;
        private string _nome;

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

        public virtual TipoGrade TipoGrade
        {
            get
            {
                return _tipoGrade;
            }

            set
            {
                _tipoGrade = value;
                OnPropertyChanged("TipoGrade");
            }
        }

        public virtual string Nome
        {
            get
            {
                return _nome?.ToUpper();
            }

            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
            }
        }
        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}

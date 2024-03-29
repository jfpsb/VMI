﻿using System;

namespace SincronizacaoVMI.Model
{
    public class TipoGrade : AModel, IModel
    {
        private int _id;
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

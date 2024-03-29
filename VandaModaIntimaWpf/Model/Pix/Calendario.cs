﻿using System;

namespace VandaModaIntimaWpf.Model.Pix
{
    public class Calendario : AModel, IModel
    {
        private int _id;
        private DateTime _criacao;
        private DateTime _apresentacao;
        private int _expiracao;

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
        public virtual DateTime Criacao
        {
            get => _criacao;
            set
            {
                _criacao = value;
                OnPropertyChanged("Criacao");
                OnPropertyChanged("CriacaoLocalTime");
            }
        }
        public virtual DateTime CriacaoLocalTime
        {
            get => _criacao.ToLocalTime();
        }
        public virtual DateTime Apresentacao
        {
            get => _apresentacao;
            set
            {
                _apresentacao = value;
                OnPropertyChanged("Apresentacao");
                OnPropertyChanged("ApresentacaoLocalTime");
            }
        }
        public virtual DateTime ApresentacaoLocalTime
        {
            get => _apresentacao.ToLocalTime();
        }
        public virtual int Expiracao
        {
            get => _expiracao;
            set
            {
                _expiracao = value;
                OnPropertyChanged("Expiracao");
            }
        }

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual void InicializaLazyLoad()
        {
            throw new NotImplementedException();
        }
    }
}

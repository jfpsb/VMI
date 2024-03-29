﻿using System;

namespace SincronizacaoVMI.Model
{
    public class Parcela : AModel, IModel
    {
        private int _id;
        private Adiantamento _adiantamento;
        private int _numero;
        private double _valor;
        private bool _paga;
        private bool _statusPagaAtual;
        private int _mes;
        private int _ano;
        private DateTime? _pagaem;

        public virtual Adiantamento Adiantamento
        {
            get => _adiantamento;
            set
            {
                _adiantamento = value;
                OnPropertyChanged("Adiantamento");
            }
        }
        public virtual double Valor
        {
            get => Math.Round(_valor, 2);
            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }
        public virtual bool Paga
        {
            get => _paga;
            set
            {
                _paga = value;
                OnPropertyChanged("Paga");
            }
        }
        /// <summary>
        /// Usada em GerenciarParcelasVM Para Guardar O Estado Provisório De Paga Ou Não Paga Na Lista
        /// </summary>
        public virtual bool StatusPagaAtual
        {
            get
            {
                if (Paga)
                    return true;

                return _statusPagaAtual;
            }

            set
            {
                _statusPagaAtual = value;
                OnPropertyChanged("StatusPagaAtual");
            }
        }
        public virtual int Numero
        {
            get => _numero;
            set
            {
                _numero = value;
                OnPropertyChanged("Numero");
            }
        }

        public virtual int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
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
        public virtual DateTime? PagaEm
        {
            get => _pagaem;
            set
            {
                _pagaem = value;
                OnPropertyChanged("PagaEm");
            }
        }
        public virtual object GetIdentifier()
        {
            return _id;
        }
    }
}

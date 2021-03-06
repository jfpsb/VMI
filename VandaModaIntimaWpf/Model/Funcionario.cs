﻿using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Funcionario : AModel, IModel
    {
        private string _cpf;
        private Loja _loja;
        private string _nome;
        private string _endereco;
        private string _telefone;
        private string _chavePIX;
        private bool _recebePassagem;
        private IList<Adiantamento> _adiantamentos = new List<Adiantamento>();
        private IList<Bonus> _bonus = new List<Bonus>();
        private IList<ContaBancaria> _contasBancarias = new List<ContaBancaria>();
        private IList<ChavePix> _chavesPix = new List<ChavePix>();
        public enum Colunas
        {
            Cpf = 1,
            Nome = 2,
            Endereco = 3,
            Salario = 4,
            Telefone = 5
        }

        [JsonIgnore]
        public virtual string GetContextMenuHeader => _cpf + " - " + _nome;

        [JsonIgnore]
        public virtual Dictionary<string, string> DictionaryIdentifier
        {
            get
            {
                var dic = new Dictionary<string, string>
                {
                    { "Cpf", Cpf }
                };

                return dic;
            }
        }

        public virtual string Cpf
        {
            get => _cpf;
            set
            {
                _cpf = value;
                OnPropertyChanged("Cpf");
            }
        }
        public virtual string Nome
        {
            get => _nome?.ToUpper();
            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
            }
        }
        public virtual string Endereco
        {
            get => _endereco?.ToUpper();
            set
            {
                _endereco = value;
                OnPropertyChanged("Endereco");
            }
        }
        public virtual string Telefone
        {
            get => _telefone;
            set
            {
                _telefone = value;
                OnPropertyChanged("Telefone");
            }
        }

        public virtual Loja Loja
        {
            get => _loja;
            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
            }
        }

        public virtual string ChavePIX
        {
            get => _chavePIX;
            set
            {
                _chavePIX = value;
                OnPropertyChanged("ChavePIX");
            }
        }

        public virtual IList<Adiantamento> Adiantamentos
        {
            get => _adiantamentos;
            set
            {
                _adiantamentos = value;
                OnPropertyChanged("Adiantamentos");
            }
        }
        public virtual IList<Bonus> Bonus
        {
            get => _bonus;
            set
            {
                _bonus = value;
                OnPropertyChanged("Bonus");
            }
        }
        public virtual bool RecebePassagem
        {
            get => _recebePassagem;
            set
            {
                _recebePassagem = value;
                OnPropertyChanged("RecebePassagem");
            }
        }

        public virtual IList<ContaBancaria> ContasBancarias
        {
            get => _contasBancarias;
            set
            {
                _contasBancarias = value;
                OnPropertyChanged("ContasBancarias");
            }
        }
        public virtual IList<ChavePix> ChavesPix
        {
            get => _chavesPix;
            set
            {
                _chavesPix = value;
                OnPropertyChanged("ChavesPix");
            }
        }

        public virtual object GetIdentifier()
        {
            return _cpf;
        }

        public virtual bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
        public virtual void InicializaLazyLoad()
        {
            if (!NHibernateUtil.IsInitialized(Loja))
            {
                NHibernateUtil.Initialize(Loja);
            }

            if(!NHibernateUtil.IsInitialized(ChavesPix))
            {
                NHibernateUtil.Initialize(ChavesPix);
            }

            if (!NHibernateUtil.IsInitialized(ContasBancarias))
            {
                NHibernateUtil.Initialize(ContasBancarias);
            }
        }
    }
}

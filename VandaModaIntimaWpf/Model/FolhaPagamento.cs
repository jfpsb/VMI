﻿using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model
{
    public class FolhaPagamento : AModel, IModel
    {
        private int _id;
        private int _mes;
        private int _ano;
        private Funcionario _funcionario;
        private bool _fechada;
        private double _salarioLiquido;
        private string _observacao;
        private double _metaVenda;
        private double _totalVendido;
        private IList<Bonus> _bonus = new List<Bonus>();
        private IList<Parcela> _parcelas = new List<Parcela>();

        [JsonIgnore]
        public virtual string GetContextMenuHeader => $"{MesReferencia} - {Funcionario.Nome}";

        [JsonIgnore]
        public virtual Dictionary<string, string> DictionaryIdentifier
        {
            get
            {
                var dic = new Dictionary<string, string>
                {
                    { "Id", Id.ToString() }
                };

                return dic;
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

        [JsonIgnore]
        public virtual string MesReferencia
        {
            get => new DateTime(Ano, Mes, 1).ToString("MM/yyyy");
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
        public virtual double ValorATransferir
        {
            get
            {
                var atransferir = SalarioLiquido + TotalBonus - TotalAdiantamentos;
                return atransferir;
            }
        }
        public virtual double TotalAdiantamentos
        {
            get
            {
                return Parcelas.Where(w => w.PagaEm == null || w.PagaEm == Vencimento).Sum(s => s.Valor);
            }
        }
        public virtual bool Fechada
        {
            get => _fechada;
            set
            {
                _fechada = value;
                OnPropertyChanged("Fechada");
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

        [JsonIgnore]
        public virtual IList<Parcela> Parcelas
        {
            get => _parcelas;
            set
            {
                _parcelas = value;
                OnPropertyChanged("Parcelas");
            }
        }

        [JsonIgnore]
        public virtual IList<Bonus> Bonus
        {
            get
            {
                return _bonus;
            }
            set
            {
                _bonus = value;
                OnPropertyChanged("Bonus");
            }
        }

        [JsonIgnore]
        public virtual double TotalBonus
        {
            get
            {
                return Bonus.Where(w => w.PagoEmFolha == false).Sum(s => s.Valor);
            }
        }

        public virtual DateTime Vencimento
        {
            get
            {
                DateTime mesSeguinteFolha = new DateTime(Ano, Mes, 5).AddMonths(1);
                return DateTimeUtil.RetornaDataUtil(5, mesSeguinteFolha.Month, mesSeguinteFolha.Year);
            }
        }

        public virtual double SalarioLiquido
        {
            get => _salarioLiquido;
            set
            {
                _salarioLiquido = value;
                OnPropertyChanged("SalarioLiquido");
            }
        }

        public virtual string Observacao
        {
            get => _observacao;
            set
            {
                _observacao = value;
                OnPropertyChanged("Observacao");
            }
        }

        public virtual double MetaDeVenda
        {
            get => _metaVenda;
            set
            {
                _metaVenda = value;
                OnPropertyChanged("MetaDeVenda");
            }
        }
        public virtual double TotalVendido
        {
            get => _totalVendido;
            set
            {
                _totalVendido = value;
                OnPropertyChanged("TotalVendido");
            }
        }

        public virtual object GetIdentifier()
        {
            return _id;
        }

        public virtual void InicializaLazyLoad()
        {
            if (!NHibernateUtil.IsInitialized(Funcionario))
            {
                NHibernateUtil.Initialize(Funcionario);
            }
        }

        public virtual bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

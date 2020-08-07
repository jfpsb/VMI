﻿using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class FolhaPagamento : ObservableObject, ICloneable, IModel
    {
        private string _id;
        private int _mes;
        private int _ano;
        private Funcionario _funcionario;
        private double _valor;
        private double _valorAPagar;
        private bool _fechada;
        private IList<Parcela> _parcelas = new List<Parcela>();
        private IList<Bonus> _bonus = new List<Bonus>();

        public string GetContextMenuHeader => _mes + "/" + _ano + " - " + _funcionario.Nome;

        public Dictionary<string, string> DictionaryIdentifier
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
        public string MesReferencia
        {
            get => _mes + "/" + _ano;
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
        public double Valor
        {
            get => Math.Round(_valor, 2);
            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }
        public bool Fechada
        {
            get => _fechada;
            set
            {
                _fechada = value;
                OnPropertyChanged("Fechada");
            }
        }
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public IList<Parcela> Parcelas
        {
            get => _parcelas;
            set
            {
                _parcelas = value;
                OnPropertyChanged("Parcelas");
            }
        }

        public double ValorAPagar
        {
            get
            {
                _valorAPagar = _funcionario.Salario;

                foreach (Parcela parcela in _parcelas)
                {
                    _valorAPagar -= parcela.Valor;
                }

                foreach (Bonus bonus in Bonus)
                {
                    _valorAPagar += bonus.Valor;
                }

                return Math.Round(_valorAPagar, 2);
            }
        }

        public double TotalAdiantamentos
        {
            get
            {
                double total = 0;

                foreach (var p in Parcelas)
                {
                    total += p.Valor;
                }

                return Math.Round(total, 2);
            }
        }

        public IList<Bonus> Bonus
        {
            get => _bonus;
            set
            {
                _bonus = value;
                OnPropertyChanged("Bonus");
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

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}

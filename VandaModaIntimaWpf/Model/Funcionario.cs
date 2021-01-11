using Newtonsoft.Json;
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
        private double _salario;
        private string _telefone;
        private string _chavePIX;
        private bool _salarioFamilia;
        private bool _recebePassagem;
        private int _numDependentes;
        private IList<Adiantamento> _adiantamentos = new List<Adiantamento>();
        private IList<Bonus> _bonus = new List<Bonus>();
        public enum Colunas
        {
            Cpf = 1,
            Nome = 2,
            Endereco = 3,
            Salario = 4,
            Telefone = 5
        }

        [JsonIgnore]
        public string GetContextMenuHeader => _cpf + " - " + _nome;

        [JsonIgnore]
        public Dictionary<string, string> DictionaryIdentifier
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

        public string Cpf
        {
            get => _cpf;
            set
            {
                _cpf = value;
                OnPropertyChanged("Cpf");
            }
        }
        public string Nome
        {
            get => _nome?.ToUpper();
            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
            }
        }
        public string Endereco
        {
            get => _endereco?.ToUpper();
            set
            {
                _endereco = value;
                OnPropertyChanged("Endereco");
            }
        }
        public double Salario
        {
            get => _salario;
            set
            {
                _salario = value;
                OnPropertyChanged("Salario");
            }
        }
        public string Telefone
        {
            get => _telefone;
            set
            {
                _telefone = value;
                OnPropertyChanged("Telefone");
            }
        }

        public Loja Loja
        {
            get => _loja;
            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
            }
        }

        public string ChavePIX
        {
            get => _chavePIX;
            set
            {
                _chavePIX = value;
                OnPropertyChanged("ChavePIX");
            }
        }

        public IList<Adiantamento> Adiantamentos
        {
            get => _adiantamentos;
            set
            {
                _adiantamentos = value;
                OnPropertyChanged("Adiantamentos");
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

        public bool SalarioFamilia
        {
            get => _salarioFamilia;
            set
            {
                _salarioFamilia = value;
                OnPropertyChanged("SalarioFamilia");
            }
        }
        public int NumDependentes
        {
            get => _numDependentes;
            set
            {
                _numDependentes = value;
                OnPropertyChanged("NumDependentes");
            }
        }

        public bool RecebePassagem
        {
            get => _recebePassagem;
            set
            {
                _recebePassagem = value;
                OnPropertyChanged("RecebePassagem");
            }
        }

        public object GetIdentifier()
        {
            return _cpf;
        }

        public bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }

        public string CouchDbId()
        {
            return Cpf?.ToString();
        }

        public void InicializaLazyLoad()
        {
            if (!NHibernateUtil.IsInitialized(Loja))
            {
                NHibernateUtil.Initialize(Loja);
            }
        }
    }
}

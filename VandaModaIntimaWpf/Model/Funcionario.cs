using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VandaModaIntimaWpf.Model
{
    public class Funcionario : AModel, IModel
    {
        private string _cpf;
        private Loja _loja;
        private Loja _lojaTrabalho;
        private Funcao _funcao;
        private string _nome;
        private string _endereco;
        private string _telefone;
        private bool _recebePassagem;
        private bool _recebeValeAlimentacao;
        private string _email;
        private string _pisPasepNit;
        private string _ctps;
        private DateTime? _admissao;
        private DateTime? _demissao;
        private double _salario;
        private string _senha;
        private IList<Adiantamento> _adiantamentos = new List<Adiantamento>();
        private IList<Bonus> _bonus = new List<Bonus>();
        private IList<ContaBancaria> _contasBancarias = new List<ContaBancaria>();
        private IList<ChavePix> _chavesPix = new List<ChavePix>();
        private IList<Ferias> _ferias = new List<Ferias>();

        private string regularmentePropriedade;

        public enum Colunas
        {
            Admissao = 1,
            Cpf = 2,
            Ctps = 3,
            PIS = 4,
            Nome = 5,
            LojaContratado = 6,
            LojaTrabalho = 7,
            Telefone = 8,
            Email = 9,
            Endereco = 10
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
                if (value != null)
                {
                    _cpf = Regex.Replace(value, "[^0-9]", string.Empty);
                    OnPropertyChanged("Cpf");
                }
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

        public virtual bool RecebeValeAlimentacao
        {
            get => _recebeValeAlimentacao;
            set
            {
                _recebeValeAlimentacao = value;
                OnPropertyChanged("RecebeValeAlimentacao");
            }
        }

        public virtual bool RegularmenteFlag
        {
            get
            {
                return (bool)GetType().GetProperty(regularmentePropriedade).GetValue(this);
            }
        }

        public virtual DateTime? Admissao
        {
            get => _admissao;
            set
            {
                _admissao = value;
                OnPropertyChanged("Admissao");
            }
        }

        public virtual string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }
        public virtual string PisPasepNit
        {
            get => _pisPasepNit;
            set
            {
                _pisPasepNit = value;
                OnPropertyChanged("PisPasepNit");
            }
        }
        public virtual string Ctps
        {
            get => _ctps;
            set
            {
                _ctps = value;
                OnPropertyChanged("Ctps");
            }
        }

        public virtual string SerieCtps
        {
            get => Ctps?.Substring(Ctps.Length - 4);
        }
        public virtual DateTime? Demissao
        {
            get => _demissao;
            set
            {
                _demissao = value;
                OnPropertyChanged("Demissao");
            }
        }

        public virtual double Salario
        {
            get => _salario;
            set
            {
                _salario = value;
                OnPropertyChanged("Salario");
            }
        }
        public virtual Loja LojaTrabalho
        {
            get => _lojaTrabalho;
            set
            {
                _lojaTrabalho = value;
                OnPropertyChanged("LojaTrabalho");
            }
        }

        public virtual IList<Ferias> Ferias
        {
            get
            {
                return _ferias;
            }

            set
            {
                _ferias = value;
                OnPropertyChanged("Ferias");
            }
        }

        public virtual string Senha
        {
            get
            {
                return _senha;
            }

            set
            {
                _senha = value;
                OnPropertyChanged("Senha");
            }
        }

        public virtual Funcao Funcao
        {
            get
            {
                return _funcao;
            }

            set
            {
                _funcao = value;
                OnPropertyChanged("Funcao");
            }
        }

        public virtual void AddChavePix(ChavePix chave)
        {
            chave.Funcionario = this;
            ChavesPix.Add(chave);
        }

        public virtual void AddContaBancaria(ContaBancaria contaBancaria)
        {
            contaBancaria.Funcionario = this;
            ContasBancarias.Add(contaBancaria);
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

            if (!NHibernateUtil.IsInitialized(ChavesPix))
            {
                NHibernateUtil.Initialize(ChavesPix);
            }

            if (!NHibernateUtil.IsInitialized(ContasBancarias))
            {
                NHibernateUtil.Initialize(ContasBancarias);
            }
        }

        public virtual void SetRegularmentePropriedade(string prop)
        {
            regularmentePropriedade = prop;
        }
    }
}

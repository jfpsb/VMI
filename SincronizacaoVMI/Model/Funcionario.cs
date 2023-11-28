using System;
using System.Text.RegularExpressions;

namespace SincronizacaoVMI.Model
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
        public virtual bool RecebePassagem
        {
            get => _recebePassagem;
            set
            {
                _recebePassagem = value;
                OnPropertyChanged("RecebePassagem");
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

        public virtual object GetIdentifier()
        {
            return Cpf;
        }
    }
}

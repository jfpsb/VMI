using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Funcionario : ObservableObject, ICloneable, IModel
    {
        private string _cpf;
        private Loja _loja;
        private string _nome;
        private string _endereco;
        private double _salario;
        private string _telefone;
        private IList<FolhaPagamento> _folhaPagamentos = new List<FolhaPagamento>();
        public enum Colunas
        {
            Cpf = 1,
            Nome = 2,
            Endereco = 3,
            Salario = 4,
            Telefone = 5
        }

        public string GetContextMenuHeader => _cpf + " - " + _nome;

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
            get => _nome;
            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
            }
        }
        public string Endereco
        {
            get => _endereco;
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

        public IList<FolhaPagamento> FolhaPagamentos
        {
            get => _folhaPagamentos;
            set
            {
                _folhaPagamentos = value;
                OnPropertyChanged("FolhaPagamentos");
            }
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public object GetIdentifier()
        {
            return _cpf;
        }

        public bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

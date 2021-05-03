using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class ContaBancaria : AModel, IModel
    {
        private long _id;
        private Funcionario _funcionario;
        private Banco _banco;
        private string _agencia;
        private string _operacao;
        private string _conta;
        public Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public string GetContextMenuHeader => $"{Banco.Nome} - Ag: {Agencia}; Op: {Operacao}; Conta: {Conta}";
        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
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
        public Banco Banco
        {
            get => _banco;
            set
            {
                _banco = value;
                OnPropertyChanged("Banco");
            }
        }
        public string Agencia
        {
            get => _agencia;
            set
            {
                _agencia = value;
                OnPropertyChanged("Agencia");
            }
        }
        public string Operacao
        {
            get => _operacao;
            set
            {
                _operacao = value;
                OnPropertyChanged("Operacao");
            }
        }
        public string Conta
        {
            get => _conta;
            set
            {
                _conta = value;
                OnPropertyChanged("Conta");
            }
        }

        public object GetIdentifier()
        {
            return Id;
        }

        public void InicializaLazyLoad()
        {
            throw new NotImplementedException();
        }

        public bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

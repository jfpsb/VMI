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
        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => $"{Banco.Nome} - Ag: {Agencia}; Op: {Operacao}; Conta: {Conta}";
        public virtual long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
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
        public virtual Banco Banco
        {
            get => _banco;
            set
            {
                _banco = value;
                OnPropertyChanged("Banco");
            }
        }
        public virtual string Agencia
        {
            get => _agencia;
            set
            {
                _agencia = value;
                OnPropertyChanged("Agencia");
            }
        }
        public virtual string Operacao
        {
            get => _operacao;
            set
            {
                _operacao = value;
                OnPropertyChanged("Operacao");
            }
        }
        public virtual string Conta
        {
            get => _conta;
            set
            {
                _conta = value;
                OnPropertyChanged("Conta");
            }
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual void InicializaLazyLoad()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

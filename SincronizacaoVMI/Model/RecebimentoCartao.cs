using System;

namespace SincronizacaoVMI.Model
{
    public class RecebimentoCartao : AModel, IModel
    {
        private int _id;
        private int _mes;
        private int _ano;
        private Loja _loja;
        private OperadoraCartao _operadoraCartao;
        private Banco _banco;
        private double _recebido;
        private double _valorOperadora;
        private string _observacao;

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

        public virtual Loja Loja
        {
            get => _loja;
            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
            }
        }
        public virtual OperadoraCartao OperadoraCartao
        {
            get => _operadoraCartao;
            set
            {
                _operadoraCartao = value;
                OnPropertyChanged("OperadoraCartao");
            }
        }
        public virtual double Recebido
        {
            get => _recebido;
            set
            {
                _recebido = value;
                OnPropertyChanged("Recebido");
            }
        }
        public virtual double ValorOperadora
        {
            get => _valorOperadora;
            set
            {
                _valorOperadora = value;
                OnPropertyChanged("ValorOperadora");
                OnPropertyChanged("Diferenca");
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
        public virtual Banco Banco
        {
            get => _banco;
            set
            {
                _banco = value;
                OnPropertyChanged("Banco");
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
        public virtual object GetIdentifier()
        {
            return Id;
        }
        public virtual void Copiar(object source)
        {
            throw new NotImplementedException();
        }
    }
}

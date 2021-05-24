using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class RecebimentoCartao : AModel, IModel
    {
        private long _id;
        private int _mes;
        private int _ano;
        private Loja _loja;
        private OperadoraCartao _operadoraCartao;
        private Banco _banco;
        private double _recebido;
        private double _valorOperadora;
        private string _observacao;

        [JsonIgnore]
        public virtual Dictionary<string, string> DictionaryIdentifier
        {
            get
            {
                var dic = new Dictionary<string, string>
                {
                    { "Mes", Mes.ToString() },
                    { "Ano", Ano.ToString() },
                    { "Loja", Loja.Cnpj },
                    { "OperadoraCartao", OperadoraCartao.Nome },
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
        public virtual string MesAno => $"{Mes}/{Ano}";

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

        [JsonIgnore]
        public virtual double Diferenca
        {
            get
            {
                double vOperadora = Math.Round(_valorOperadora, 2, MidpointRounding.AwayFromZero);
                double vRecebido = Math.Round(_recebido, 2, MidpointRounding.AwayFromZero);
                return Math.Round(vRecebido - vOperadora, 2, MidpointRounding.AwayFromZero);
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

        public virtual bool IsIdentical(object obj)
        {
            if (obj != null && obj.GetType() == typeof(RecebimentoCartao))
            {
                RecebimentoCartao recebimentoCartao = (RecebimentoCartao)obj;
                return recebimentoCartao.Mes.Equals(Mes)
                       && recebimentoCartao.Ano.Equals(Ano)
                       && recebimentoCartao.Loja.Cnpj.Equals(Loja.Cnpj)
                       && recebimentoCartao.OperadoraCartao.Nome.Equals(OperadoraCartao.Nome)
                       && recebimentoCartao.Recebido.Equals(Recebido)
                       && recebimentoCartao.ValorOperadora.Equals(ValorOperadora)
                       && recebimentoCartao.Observacao.Equals(Observacao);
            }

            return false;
        }

        [JsonIgnore]
        public virtual string GetContextMenuHeader => $"{MesAno} - {Loja.Nome}";

        public virtual Banco Banco
        {
            get => _banco;
            set
            {
                _banco = value;
                OnPropertyChanged("Banco");
            }
        }

        public virtual long Id
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

        public virtual void InicializaLazyLoad()
        {
            throw new NotImplementedException("RecebimentoCartao Não Possui Propriedades com Lazy Loading");
        }
    }
}

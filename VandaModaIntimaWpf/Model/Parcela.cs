using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Parcela : AModel, IModel
    {
        private int _id;
        private Adiantamento _adiantamento;
        private int _numero;
        private double _valor;
        private bool _paga;
        private bool _statusPagaAtual;
        private int _mes;
        private int _ano;
        private DateTime? _pagaem;

        [JsonIgnore]
        public virtual string GetContextMenuHeader => throw new NotImplementedException();

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

        public virtual Adiantamento Adiantamento
        {
            get => _adiantamento;
            set
            {
                _adiantamento = value;
                OnPropertyChanged("Adiantamento");
            }
        }
        public virtual double Valor
        {
            get => Math.Round(_valor, 2);
            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }
        public virtual bool Paga
        {
            get => _paga;
            set
            {
                _paga = value;
                OnPropertyChanged("Paga");
            }
        }
        /// <summary>
        /// Usada em GerenciarParcelasVM Para Guardar O Estado Provisório De Paga Ou Não Paga Na Lista
        /// </summary>
        public virtual bool StatusPagaAtual
        {
            get
            {
                if (Paga)
                    return true;

                return _statusPagaAtual;
            }

            set
            {
                _statusPagaAtual = value;
                OnPropertyChanged("StatusPagaAtual");
            }
        }
        public virtual int Numero
        {
            get => _numero;
            set
            {
                _numero = value;
                OnPropertyChanged("Numero");
            }
        }
        public virtual string NumeroComTotal
        {
            get => $"{Numero}/{Adiantamento.Parcelas.Count}";
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
        public virtual int Mes
        {
            get => _mes;
            set
            {
                _mes = value;
                OnPropertyChanged("Mes");
                OnPropertyChanged("Vencimento");
                OnPropertyChanged("FolhaReferencia");
            }
        }
        public virtual int Ano
        {
            get => _ano;
            set
            {
                _ano = value;
                OnPropertyChanged("Ano");
                OnPropertyChanged("Vencimento");
                OnPropertyChanged("FolhaReferencia");
            }
        }

        public virtual DateTime? PagaEm
        {
            get => _pagaem;
            set
            {
                _pagaem = value;
                OnPropertyChanged("PagaEm");
            }
        }

        [JsonIgnore]
        public virtual string Vencimento
        {
            get
            {
                DateTime refer = new DateTime(Ano, Mes, 1);
                return refer.AddMonths(1).ToString("MM/yyyy");
            }
        }

        public virtual string FolhaReferencia
        {
            get => $"{Mes}/{Ano}";
        }

        public virtual object GetIdentifier()
        {
            return _id;
        }

        public virtual void InicializaLazyLoad()
        {
            if (!NHibernateUtil.IsInitialized(Adiantamento))
            {
                NHibernateUtil.Initialize(Adiantamento);
            }
        }

        public virtual bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

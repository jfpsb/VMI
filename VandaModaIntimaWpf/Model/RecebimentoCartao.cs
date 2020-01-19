using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    class RecebimentoCartao : ObservableObject, ICloneable, IModel
    {
        private int mes { get; set; }
        private int ano { get; set; }
        private Loja loja { get; set; }
        private OperadoraCartao operadoraCartao { get; set; }
        private double valor { get; set; }
        public virtual int Mes
        {
            get { return mes; }
            set
            {
                mes = value;
                OnPropertyChanged("Mes");
            }
        }
        public virtual int Ano
        {
            get { return ano; }
            set
            {
                ano = value;
                OnPropertyChanged("Ano");
            }
        }
        public virtual Loja Loja
        {
            get { return loja; }
            set
            {
                loja = value;
                OnPropertyChanged("Loja");
            }
        }
        public virtual OperadoraCartao OperadoraCartao
        {
            get { return operadoraCartao; }
            set
            {
                operadoraCartao = value;
                OnPropertyChanged("OperacaoCartao");
            }
        }
        public virtual double Valor
        {
            get { return valor; }
            set
            {
                valor = value;
                OnPropertyChanged("Valor");
            }
        }

        public virtual string GetContextMenuHeader { get => $"{Mes}/{Ano}"; }

        public virtual object Clone()
        {
            throw new NotImplementedException();
        }

        public virtual object GetId()
        {
            return new List<object> { mes, ano, loja, operadoraCartao };
        }

        public override bool Equals(object obj)
        {
            if(obj.GetType() == typeof(RecebimentoCartao))
            {
                RecebimentoCartao rc = (RecebimentoCartao)obj;
                if (Mes == rc.Mes && Ano == rc.Ano && Loja.Cnpj == rc.Loja.Cnpj && OperadoraCartao.Nome == rc.OperadoraCartao.Nome)
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Mes.GetHashCode() + Ano.GetHashCode() + Loja.GetHashCode() + OperadoraCartao.GetHashCode();
        }
    }
}

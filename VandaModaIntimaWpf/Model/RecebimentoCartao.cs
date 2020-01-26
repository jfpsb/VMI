using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class RecebimentoCartao : ObservableObject, ICloneable, IModel
    {
        private int mes { get; set; }
        private int ano { get; set; }
        private Loja loja { get; set; }
        private OperadoraCartao operadoraCartao { get; set; }
        private double recebido { get; set; }
        private double valorOperadora { get; set; }
        private string observacao { get; set; }
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
        public virtual string MesAno
        {
            get { return $"{Mes}/{Ano}"; }
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
                OnPropertyChanged("OperadoraCartao");
            }
        }
        public virtual double Recebido
        {
            get { return recebido; }
            set
            {
                recebido = value;
                OnPropertyChanged("Recebido");
            }
        }
        public virtual double ValorOperadora
        {
            get { return valorOperadora; }
            set
            {
                valorOperadora = value;
                OnPropertyChanged("ValorOperadora");
                OnPropertyChanged("Diferenca");
            }
        }
        public virtual double Diferenca
        {
            get
            {
                double v1 = Math.Round(valorOperadora, 2, MidpointRounding.AwayFromZero);
                double v2 = Math.Round(recebido, 2, MidpointRounding.AwayFromZero);
                return Math.Round(v1 - v2, 2, MidpointRounding.AwayFromZero);
            }
        }
        public virtual string Observacao
        {
            get { return observacao; }
            set
            {
                observacao = value;
                OnPropertyChanged("Observacao");
            }
        }
        public virtual string GetContextMenuHeader { get => $"{MesAno} - {Loja.Nome}"; }

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
            if (obj.GetType() == typeof(RecebimentoCartao))
            {
                RecebimentoCartao rc = (RecebimentoCartao)obj;
                if (Mes == rc.Mes && Ano == rc.Ano && Loja.Cnpj == rc.Loja.Cnpj && OperadoraCartao.Nome == rc.OperadoraCartao.Nome)
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 0;

            if (Loja != null)
                hash += Loja.GetHashCode();

            if (OperadoraCartao != null)
                hash += OperadoraCartao.GetHashCode();

            return Mes.GetHashCode() + Ano.GetHashCode() + hash;
        }
    }
}

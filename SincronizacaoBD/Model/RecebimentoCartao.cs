using System;
using System.Xml.Serialization;

namespace SincronizacaoBD.Model
{
    [XmlRoot(ElementName = "EntidadeSalva")]
    public class RecebimentoCartao
    {
        private int mes { get; set; }
        private int ano { get; set; }
        private Loja loja { get; set; }
        private OperadoraCartao operadoraCartao { get; set; }
        private double recebido { get; set; }
        private double valorOperadora { get; set; }
        private string observacao { get; set; }
        private DateTime lastUpdate { get; set; } = DateTime.Now;
        public virtual int Mes
        {
            get { return mes; }
            set
            {
                mes = value;
            }
        }
        public virtual int Ano
        {
            get { return ano; }
            set
            {
                ano = value;
            }
        }
        public virtual Loja Loja
        {
            get { return loja; }
            set
            {
                loja = value;
            }
        }
        public virtual OperadoraCartao OperadoraCartao
        {
            get { return operadoraCartao; }
            set
            {
                operadoraCartao = value;
            }
        }
        public virtual double Recebido
        {
            get { return recebido; }
            set
            {
                recebido = value;
            }
        }
        public virtual double ValorOperadora
        {
            get { return valorOperadora; }
            set
            {
                valorOperadora = value;
            }
        }
        public virtual string Observacao
        {
            get { return observacao; }
            set
            {
                observacao = value;
            }
        }
        public virtual DateTime LastUpdate
        {
            get { return lastUpdate; }
            set
            {
                lastUpdate = value;
            }
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

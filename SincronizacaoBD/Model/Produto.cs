using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SincronizacaoBD.Model
{
    public class Produto :  IModel
    {
        private string cod_barra;
        private Fornecedor fornecedor;
        private Marca marca;
        private string descricao;
        private double preco;
        private string ncm;
        private IList<string> codigos = new List<string>();

        public virtual string Cod_Barra
        {
            get
            {
                return cod_barra;
            }
            set
            {
                cod_barra = value;
            }
        }
        public virtual Fornecedor Fornecedor
        {
            get
            {
                return fornecedor;
            }

            set
            {
                fornecedor = value;
            }
        }

        public virtual Marca Marca
        {
            get
            {
                return marca;
            }
            set
            {
                marca = value;
            }
        }

        public virtual string Descricao
        {
            get
            {
                return descricao;
            }

            set
            {
                descricao = value.ToUpper();
            }
        }

        public virtual double Preco
        {
            get { return preco; }
            set
            {
                preco = value;
            }
        }

        public virtual string Ncm
        {
            get { return ncm; }
            set
            {
                ncm = value;
            }
        }
        
        public virtual IList<string> Codigos
        {
            get
            {
                return codigos;
            }
            set
            {
                codigos = value;
            }
        }

        [JsonIgnore]
        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual object GetIdentifier()
        {
            return Cod_Barra;
        }
    }
}

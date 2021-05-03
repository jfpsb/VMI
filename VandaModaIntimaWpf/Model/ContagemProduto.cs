using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class ContagemProduto : AModel, IModel
    {
        private long _id;
        private Contagem _contagem;
        private Produto _produto;
        private int _quant;

        public bool IsIdentical(object obj)
        {
            if (obj != null && obj.GetType() == typeof(ContagemProduto))
            {
                ContagemProduto contagemProduto = (ContagemProduto)obj;

                return contagemProduto.Id.Equals(Id)
                       && contagemProduto.Contagem.Equals(Contagem)
                       && contagemProduto.Produto.Equals(Produto)
                       && contagemProduto.Quant.Equals(Quant);
            }

            return false;
        }

        [JsonIgnore]
        public virtual string GetContextMenuHeader => $"{Produto.CodBarra}; Quantidade: {Quant}";

        [JsonIgnore]
        public Dictionary<string, string> DictionaryIdentifier
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

        [JsonProperty(PropertyName = "MySqlId")]
        public virtual long Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public virtual Produto Produto
        {
            get
            {
                return _produto;
            }

            set
            {
                _produto = value;
                OnPropertyChanged("Produto");
            }
        }

        public virtual int Quant
        {
            get
            {
                return _quant;
            }

            set
            {
                _quant = value;
                OnPropertyChanged("Quant");
            }
        }

        public virtual Contagem Contagem
        {
            get
            {
                return _contagem;
            }

            set
            {
                _contagem = value;
                OnPropertyChanged("Contagem");
            }
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public void InicializaLazyLoad()
        {
            throw new NotImplementedException("ContagemProduto Não Possui Propriedades Que Usam Lazy Loading");
        }
    }
}

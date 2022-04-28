using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class ContagemProduto : AModel, IModel
    {
        private int _id;
        private Contagem _contagem;
        private ProdutoGrade _produtoGrade;
        private int _quant;

        public virtual bool IsIdentical(object obj)
        {
            if (obj != null && obj.GetType() == typeof(ContagemProduto))
            {
                ContagemProduto contagemProduto = (ContagemProduto)obj;

                return contagemProduto.Id.Equals(Id)
                       && contagemProduto.Contagem.Equals(Contagem)
                       && contagemProduto.ProdutoGrade.Equals(ProdutoGrade)
                       && contagemProduto.Quant.Equals(Quant);
            }

            return false;
        }

        [JsonIgnore]
        public virtual string GetContextMenuHeader => $"{ProdutoGrade.CodBarra}; Quantidade: {Quant}";

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

        public virtual int Id
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

        public virtual ProdutoGrade ProdutoGrade
        {
            get
            {
                return _produtoGrade;
            }

            set
            {
                _produtoGrade = value;
                OnPropertyChanged("ProdutoGrade");
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

        public virtual void InicializaLazyLoad()
        {
            throw new NotImplementedException("ContagemProduto Não Possui Propriedades Que Usam Lazy Loading");
        }
    }
}

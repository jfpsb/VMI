using Newtonsoft.Json;
using System;

namespace VandaModaIntimaWpf.Model
{
    public class ContagemProduto : ObservableObject, ICloneable, IModel
    {
        private long id;
        private Contagem contagem;
        private Produto produto;
        private int quant;

        [JsonIgnore]
        public virtual string GetContextMenuHeader { get { return $"{Produto.Cod_Barra}; Quantidade: {Quant}"; } }

        public virtual long Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public virtual Produto Produto
        {
            get
            {
                return produto;
            }

            set
            {
                produto = value;
                OnPropertyChanged("Produto");
            }
        }

        public virtual int Quant
        {
            get
            {
                return quant;
            }

            set
            {
                quant = value;
                OnPropertyChanged("Quant");
            }
        }

        [JsonIgnore]
        public virtual Contagem Contagem
        {
            get
            {
                return contagem;
            }

            set
            {
                contagem = value;
                OnPropertyChanged("Contagem");
            }
        }

        public virtual object Clone()
        {
            throw new NotImplementedException();
        }

        public virtual object GetIdentifier()
        {
            return Id.ToString();
        }
    }
}

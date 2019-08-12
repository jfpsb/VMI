using System;
using System.Collections.Generic;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto.Produto;

namespace VandaModaIntimaWpf.Model.Marca
{
    public partial class Marca : ObservableObject, ICloneable
    {
        private string nome { get; set; }

        private IList<ProdutoModel> produtos = new List<ProdutoModel>();

        public enum Colunas
        {
            Nome = 1
        }

        /// <summary>
        /// Construtor para criar placeholder de Marca para comboboxes
        /// </summary>
        /// <param name="nome">SELECIONE UMA MARCA</param>
        public Marca(string nome)
        {
            this.nome = nome;
        }

        public virtual string Nome
        {
            get { return nome; }
            set
            {
                nome = value.ToUpper();
                OnPropertyChanged("Nome");
            }
        }

        public virtual IList<ProdutoModel> Produtos
        {
            get { return produtos; }
            set
            {
                produtos = value;
                OnPropertyChanged("Produtos");
            }
        }

        public virtual object Clone()
        {
            Marca m = new Marca();

            m.Nome = Nome;

            return m;
        }
    }
}

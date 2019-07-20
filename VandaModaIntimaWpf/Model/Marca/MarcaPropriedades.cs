using System.Collections.Generic;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto.Produto;

namespace VandaModaIntimaWpf.Model.Marca
{
    public partial class Marca : ObservableObject
    {
        private long id { get; set; }
        private string nome { get; set; }

        private IList<ProdutoModel> produtos = new List<ProdutoModel>();

        /// <summary>
        /// Construtor para criar placeholder de Marca para comboboxes
        /// </summary>
        /// <param name="nome">SELECIONE UMA MARCA</param>
        public Marca(string nome)
        {
            id = 0;
            this.nome = nome;
        }

        public virtual long Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public virtual string Nome
        {
            get { return nome; }
            set
            {
                nome = value;
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
    }
}

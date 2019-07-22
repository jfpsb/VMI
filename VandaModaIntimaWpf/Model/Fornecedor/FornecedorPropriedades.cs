using System;
using System.Collections.Generic;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto.Produto;

namespace VandaModaIntimaWpf.Model.Fornecedor
{
    public partial class Fornecedor : ObservableObject, ICloneable
    {
        private string cnpj { get; set; }
        private string nome { get; set; }
        private string nomeFantasia { get; set; }
        private string email { get; set; }

        private IList<ProdutoModel> produtos = new List<ProdutoModel>();

        /// <summary>
        /// Construtor para criar placeholder de Fornecedor para comboboxes
        /// </summary>
        /// <param name="nome">SELECIONE UM FORNECEDOR</param>
        public Fornecedor(string nome)
        {
            cnpj = "0";
            this.nome = nome;
        }

        public virtual string Cnpj
        {
            get { return cnpj; }
            set
            {
                cnpj = value;
                OnPropertyChanged("Cnpj");
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

        public virtual string NomeFantasia
        {
            get { return nomeFantasia; }
            set
            {
                nomeFantasia = value;
                OnPropertyChanged("NomeFantasia");
            }
        }

        public virtual string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
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
            Fornecedor f = new Fornecedor();

            f.Cnpj = Cnpj;
            f.Nome = Nome;
            f.NomeFantasia = NomeFantasia;
            f.Email = Email;

            return f;
        }
    }
}

using System;
using System.Collections.Generic;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.Model
{
    public class Fornecedor : ObservableObject, ICloneable, IModel
    {
        private string cnpj { get; set; }
        private string nome { get; set; }
        private string nomeFantasia { get; set; }
        private string email { get; set; }

        private IList<ProdutoModel> produtos = new List<ProdutoModel>();

        public enum Colunas
        {
            CNPJ = 1,
            Nome = 2,
            NomeFantasia = 3,
            Email = 4
        }

        public Fornecedor() { }

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
                if (value != null)
                {
                    nome = value.ToUpper();
                    OnPropertyChanged("Nome");
                }
            }
        }

        public virtual string NomeFantasia
        {
            get { return nomeFantasia; }
            set
            {
                if (value != null)
                {
                    nomeFantasia = value.ToUpper();
                    OnPropertyChanged("NomeFantasia");
                }
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

        public virtual string[] GetColunas()
        {
            return new string[] { "CNPJ", "Nome", "Nome Fantasia", "Email" };
        }

        public virtual object GetId()
        {
            return Cnpj;
        }
    }
}

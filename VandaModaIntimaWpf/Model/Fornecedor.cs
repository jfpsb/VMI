using SincronizacaoBD.Model;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.Model
{
    [XmlRoot(ElementName = "EntidadeSalva")]
    public class Fornecedor : ObservableObject, ICloneable, IModel
    {
        private string cnpj { get; set; }
        private string nome { get; set; }
        private string fantasia { get; set; }
        private string email { get; set; }
        private string telefone { get; set; }

        private IList<ProdutoModel> produtos = new List<ProdutoModel>();

        public enum Colunas
        {
            CNPJ = 1,
            Nome = 2,
            NomeFantasia = 3,
            Telefone = 4,
            Email = 5
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
                if (value != null)
                {
                    cnpj = Regex.Replace(value, @"[-./]", "");
                    OnPropertyChanged("Cnpj");
                }
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

        public virtual string Fantasia
        {
            get { return fantasia; }
            set
            {
                if (value != null)
                {
                    fantasia = value.ToUpper();
                    OnPropertyChanged("Fantasia");
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
        public virtual string Telefone
        {
            get { return telefone; }
            set
            {
                telefone = value;
                OnPropertyChanged("Telefone");
            }
        }

        [XmlIgnore]
        public virtual IList<ProdutoModel> Produtos
        {
            get { return produtos; }
            set
            {
                produtos = value;
                OnPropertyChanged("Produtos");
            }
        }

        public virtual string GetContextMenuHeader { get { return Nome; } }

        public virtual object Clone()
        {
            Fornecedor f = new Fornecedor();

            f.Cnpj = Cnpj;
            f.Nome = Nome;
            f.Fantasia = Fantasia;
            f.Email = Email;
            f.Telefone = Telefone;

            return f;
        }

        public virtual string[] GetColunas()
        {
            return new string[] { "CNPJ", "Nome", "Nome Fantasia", "Telefone", "Email" };
        }

        public virtual object GetId()
        {
            return Cnpj;
        }
    }
}

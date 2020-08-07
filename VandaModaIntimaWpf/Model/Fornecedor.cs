using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.Model
{
    public class Fornecedor : ObservableObject, ICloneable, IModel
    {
        private string _cnpj;
        private string _nome;
        private string _fantasia;
        private string _email;
        private string _telefone;

        private IList<ProdutoModel> _produtos = new List<ProdutoModel>();

        public enum Colunas
        {
            Cnpj = 1,
            Nome = 2,
            NomeFantasia = 3,
            Telefone = 4,
            Email = 5
        }

        public Dictionary<string, string> DictionaryIdentifier
        {
            get
            {
                var dic = new Dictionary<string, string>
                {
                    { "Cnpj", Cnpj }
                };

                return dic;
            }
        }

        public Fornecedor() { }

        /// <summary>
        /// Construtor para criar placeholder de Fornecedor para comboboxes
        /// </summary>
        /// <param name="nome">SELECIONE UM FORNECEDOR</param>
        public Fornecedor(string nome)
        {
            _nome = nome;
        }

        public virtual string Cnpj
        {
            get => _cnpj;
            set
            {
                if (value != null)
                {
                    _cnpj = Regex.Replace(value, @"[-./]", string.Empty);
                    OnPropertyChanged("Cnpj");
                }
            }
        }

        public virtual string Nome
        {
            get => _nome;
            set
            {
                if (value != null)
                {
                    _nome = value.ToUpper();
                    OnPropertyChanged("Nome");
                }
            }
        }

        public virtual string Fantasia
        {
            get => _fantasia;
            set
            {
                if (value != null)
                {
                    _fantasia = value.ToUpper();
                    OnPropertyChanged("Fantasia");
                }
            }
        }

        public virtual string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }
        public virtual string Telefone
        {
            get => _telefone;
            set
            {
                _telefone = value;
                OnPropertyChanged("Telefone");
            }
        }

        public virtual IList<ProdutoModel> Produtos
        {
            get => _produtos;
            set
            {
                _produtos = value;
                OnPropertyChanged("Produtos");
            }
        }

        public bool IsIdentical(object obj)
        {
            if (obj != null && obj.GetType() == typeof(Fornecedor))
            {
                Fornecedor fornecedor = (Fornecedor)obj;

                return fornecedor.Cnpj.Equals(Cnpj)
                       && fornecedor.Nome.Equals(Nome)
                       && fornecedor.Fantasia.Equals(Fantasia)
                       && fornecedor.Email.Equals(Email)
                       && fornecedor.Telefone.Equals(Telefone);
                ;
            }
            return false;
        }

        public virtual string GetContextMenuHeader => Nome;

        public virtual object Clone()
        {
            Fornecedor f = new Fornecedor
            {
                Cnpj = Cnpj,
                Nome = Nome,
                Fantasia = Fantasia,
                Email = Email,
                Telefone = Telefone
            };


            return f;
        }

        public virtual string[] GetColunas()
        {
            return new[] { "CNPJ", "Nome", "Nome Fantasia", "Telefone", "Email" };
        }

        public virtual object GetIdentifier()
        {
            return Cnpj;
        }

        public override string ToString()
        {
            return Cnpj?.ToString();
        }
    }
}

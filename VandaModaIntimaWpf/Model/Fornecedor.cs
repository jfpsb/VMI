﻿using Newtonsoft.Json;
using NHibernate;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.Model
{
    public class Fornecedor : AModel, IModel
    {
        private string _cnpj;
        private string _nome;
        private string _fantasia;
        private string _email;
        private string _telefone;
        private Representante _representante;

        private IList<ProdutoModel> _produtos = new List<ProdutoModel>();

        public enum Colunas
        {
            Cnpj = 1,
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
            _nome = nome;
        }

        public virtual string Cnpj
        {
            get => _cnpj;
            set
            {
                if (value != null)
                {
                    _cnpj = Regex.Replace(value, "[^0-9]", string.Empty);
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

        [JsonIgnore]
        public virtual IList<ProdutoModel> Produtos
        {
            get => _produtos;
            set
            {
                _produtos = value;
                OnPropertyChanged("Produtos");
            }
        }

        [JsonIgnore]
        public virtual string GetContextMenuHeader => $"{Nome} - {Fantasia}";

        public virtual Representante Representante
        {
            get => _representante;
            set
            {
                _representante = value;
                OnPropertyChanged("Representante");
            }
        }

        public virtual string[] GetColunas()
        {
            return new[] { "CNPJ", "Nome", "Nome Fantasia", "Telefone", "Email" };
        }

        public virtual object GetIdentifier()
        {
            return Cnpj;
        }

        public virtual void InicializaLazyLoad()
        {
            if (!NHibernateUtil.IsInitialized(Produtos))
            {
                NHibernateUtil.Initialize(Produtos);
            }
        }

        public virtual void CopiaDadosDeConsultaNaReceita(Fornecedor fornecedorConsultado)
        {
            Nome = fornecedorConsultado.Nome;
            Fantasia = fornecedorConsultado.Fantasia;
            Email = fornecedorConsultado.Email;
            Telefone = fornecedorConsultado.Telefone;
        }
    }
}

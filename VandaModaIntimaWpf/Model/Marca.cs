using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.Model
{
    public class Marca : AModel, IModel
    {
        private string _nome;
        private Fornecedor _fornecedor;
        private IList<ProdutoModel> _produtos = new List<ProdutoModel>();

        public enum Colunas
        {
            Nome = 1
        }
        public Marca() { }

        /// <summary>
        /// Construtor para criar placeholder de Marca para comboboxes
        /// </summary>
        /// <param name="nome">Placeholder para nome</param>
        public Marca(string nome)
        {
            _nome = nome;
        }

        [JsonIgnore]
        public virtual Dictionary<string, string> DictionaryIdentifier
        {
            get
            {
                var dic = new Dictionary<string, string>
                {
                    { "Nome", Nome }
                };

                return dic;
            }
        }

        public virtual string Nome
        {
            get => _nome;
            set
            {
                _nome = value.ToUpper();
                OnPropertyChanged("Nome");
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

        public virtual bool IsIdentical(object obj)
        {
            if (obj != null && obj.GetType() == typeof(Marca))
            {
                Marca marca = (Marca)obj;
                return marca.Nome.Equals(Nome)
                       && marca.Fornecedor.Equals(Fornecedor);
            }

            return false;
        }

        [JsonIgnore]
        public virtual string GetContextMenuHeader => Nome;

        public virtual Fornecedor Fornecedor
        {
            get => _fornecedor;

            set
            {
                _fornecedor = value;
                OnPropertyChanged("Fornecedor");
            }
        }

        public virtual string[] GetColunas()
        {
            return new[] { "Nome" };
        }

        public virtual object GetIdentifier()
        {
            return Nome;
        }

        public virtual void InicializaLazyLoad()
        {
            if (!NHibernateUtil.IsInitialized(Fornecedor))
            {
                NHibernateUtil.Initialize(Fornecedor);
            }

            if (!NHibernateUtil.IsInitialized(Produtos))
            {
                NHibernateUtil.Initialize(Produtos);
            }
        }
    }
}

using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.Model
{
    public class Produto : AModel, IModel, ICloneable
    {
        private long _id;
        private string _codBarra;
        private FornecedorModel _fornecedor;
        private MarcaModel _marca;
        private string _descricao;
        private string _ncm;
        private IList<ProdutoGrade> _grades = new List<ProdutoGrade>();

        public enum Colunas
        {
            CodBarra = 1,
            Descricao = 2,
            Fornecedor = 3,
            Marca = 4,
            Ncm = 5,
        }

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

        public virtual long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual string CodBarra
        {
            get => _codBarra;
            set
            {
                if (!value.Equals(_codBarra))
                {
                    _codBarra = value;
                    OnPropertyChanged("CodBarra");
                }
            }
        }
        public virtual FornecedorModel Fornecedor
        {
            get => _fornecedor;

            set
            {
                _fornecedor = value;
                OnPropertyChanged("Fornecedor");
                OnPropertyChanged("FornecedorNome");
            }
        }

        public virtual MarcaModel Marca
        {
            get => _marca;
            set
            {
                _marca = value;
                OnPropertyChanged("Marca");
                OnPropertyChanged("MarcaNome");
            }
        }

        public virtual string Descricao
        {
            get => _descricao;

            set
            {
                _descricao = value.ToUpper();
                OnPropertyChanged("Descricao");
            }
        }

        public virtual string Ncm
        {
            get => _ncm;
            set
            {
                _ncm = value;
                OnPropertyChanged("Ncm");
            }
        }

        [JsonIgnore]
        public virtual string FornecedorNome
        {
            get
            {
                if (Fornecedor != null)
                    return Fornecedor.Nome;

                return "Não Há Fornecedor";
            }
        }

        [JsonIgnore]
        public virtual string MarcaNome
        {
            get
            {
                if (Marca != null)
                    return Marca.Nome;

                return "Não Há Marca";
            }
        }

        public virtual bool IsIdentical(object obj)
        {
            if (obj != null && obj.GetType() == typeof(Produto))
            {
                Produto produto = (Produto)obj;
                return produto.CodBarra.Equals(CodBarra)
                       && produto.Fornecedor.Equals(Fornecedor)
                       && produto.Marca.Equals(Marca)
                       && produto.Descricao.Equals(Descricao)
                       && produto.Ncm.Equals(Ncm);
            }
            return false;
        }

        [JsonIgnore]
        public virtual string GetContextMenuHeader => Descricao;

        public virtual IList<ProdutoGrade> Grades
        {
            get
            {
                return _grades;
            }

            set
            {
                _grades = value;
                OnPropertyChanged("Grades");
            }
        }
        public virtual string ListarGradesString
        {
            get
            {
                string str = "";

                foreach (var g in Grades)
                {
                    str += $"{g.SubGradesToString} =>\nPreço de Custo: {g.PrecoCusto:C}\nPreço De Venda: {g.Preco:C}\n\n";
                }

                return str;
            }
        }
        public virtual string[] GetColunas()
        {
            return new[] { "Cód. de Barras", "Descrição", "Preço", "Fornecedor", "Marca", "NCM" };
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual void InicializaLazyLoad()
        {
            if (!NHibernateUtil.IsInitialized(Grades))
            {
                NHibernateUtil.Initialize(Grades);
            }
            if (!NHibernateUtil.IsInitialized(Fornecedor))
            {
                NHibernateUtil.Initialize(Fornecedor);
            }

            if (!NHibernateUtil.IsInitialized(Marca))
            {
                NHibernateUtil.Initialize(Marca);
            }
        }

        public virtual object Clone()
        {
            Produto produto = new Produto();

            produto.Id = Id;
            produto.CodBarra = CodBarra;
            produto.Descricao = Descricao;
            produto.Fornecedor = Fornecedor;
            produto.Marca = Marca;
            produto.Ncm = Ncm;
            produto.Grades = new List<ProdutoGrade>(Grades);

            return produto;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.Model
{
    public class Produto : ObservableObject, ICloneable, IModel
    {
        private string _codBarra;
        private FornecedorModel _fornecedor;
        private MarcaModel _marca;
        private string _descricao;
        private double _preco;
        private string _ncm;
        private IList<string> _codigos = new List<string>();
        public enum Colunas
        {
            CodBarra = 1,
            Descricao = 2,
            Preco = 3,
            Fornecedor = 4,
            Marca = 5,
            Ncm = 6,
            CodBarraFornecedor = 7
        }

        [JsonIgnore]
        public Dictionary<string, string> DictionaryIdentifier
        {
            get
            {
                var dic = new Dictionary<string, string>
                {
                    { "CodBarra", CodBarra }
                };

                return dic;
            }
        }

        public virtual string CodBarra
        {
            get => _codBarra;
            set
            {
                _codBarra = value;
                OnPropertyChanged("CodBarra");
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

        public virtual double Preco
        {
            get => _preco;
            set
            {
                _preco = value;
                OnPropertyChanged("Preco");
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

        public virtual IList<string> Codigos
        {
            get => _codigos;
            set
            {
                _codigos = value;
                OnPropertyChanged("Codigos");
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

        public bool IsIdentical(object obj)
        {
            if (obj != null && obj.GetType() == typeof(Produto))
            {
                Produto produto = (Produto)obj;
                return produto.CodBarra.Equals(CodBarra)
                       && produto.Fornecedor.Equals(Fornecedor)
                       && produto.Marca.Equals(Marca)
                       && produto.Descricao.Equals(Descricao)
                       && produto.Preco.Equals(Preco)
                       && produto.Ncm.Equals(Ncm);
            }
            return false;
        }

        [JsonIgnore]
        public virtual string GetContextMenuHeader => Descricao;

        public virtual object Clone()
        {
            Produto p = new Produto
            {
                CodBarra = CodBarra,
                Descricao = Descricao,
                Preco = Preco,
                Fornecedor = Fornecedor,
                Marca = Marca,
                Ncm = Ncm,
                Codigos = new List<string>(Codigos)
            };


            return p;
        }

        public virtual string[] GetColunas()
        {
            return new[] { "Cód. de Barras", "Descrição", "Preço", "Fornecedor", "Marca", "NCM", "Cód. De Barras de Fornecedor" };
        }

        public virtual object GetIdentifier()
        {
            return CodBarra;
        }

        public override string ToString()
        {
            return CodBarra;
        }
    }
}

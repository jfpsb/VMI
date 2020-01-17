using System;
using System.Collections.Generic;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.Model
{
    public class Produto : ObservableObject, ICloneable, IModel
    {
        private string cod_barra;
        private FornecedorModel fornecedor;
        private MarcaModel marca;
        private string descricao;
        private double preco;
        private IList<string> codigos = new List<string>();

        public enum Colunas
        {
            CodBarra = 1,
            Descricao = 2,
            Preco = 3,
            Fornecedor = 4,
            Marca = 5,
            CodBarraFornecedor = 6
        }

        public virtual string Cod_Barra
        {
            get
            {
                return cod_barra;
            }
            set
            {
                cod_barra = value;
                OnPropertyChanged("Cod_Barra");
            }
        }
        public virtual FornecedorModel Fornecedor
        {
            get
            {
                return fornecedor;
            }

            set
            {
                fornecedor = value;
                OnPropertyChanged("Fornecedor");
                OnPropertyChanged("FornecedorNome");
            }
        }

        public virtual MarcaModel Marca
        {
            get
            {
                return marca;
            }
            set
            {
                marca = value;
                OnPropertyChanged("Marca");
                OnPropertyChanged("MarcaNome");
            }
        }

        public virtual string Descricao
        {
            get
            {
                return descricao;
            }

            set
            {
                descricao = value.ToUpper();
                OnPropertyChanged("Descricao");
            }
        }

        public virtual double Preco
        {
            get { return preco; }
            set
            {
                preco = value;
                OnPropertyChanged("Preco");
            }
        }

        public virtual IList<string> Codigos
        {
            get
            {
                return codigos;
            }
            set
            {
                codigos = value;
                OnPropertyChanged("Codigos");
            }
        }

        public virtual string FornecedorNome
        {
            get
            {
                if (Fornecedor != null)
                    return Fornecedor.Nome;

                return "Não Há Fornecedor";
            }
        }

        public virtual string MarcaNome
        {
            get
            {
                if (Marca != null)
                    return Marca.Nome;

                return "Não Há Marca";
            }
        }

        public virtual object Clone()
        {
            Produto p = new Produto();

            p.Cod_Barra = Cod_Barra;
            p.Descricao = Descricao;
            p.Preco = Preco;
            p.Fornecedor = Fornecedor;
            p.Marca = Marca;
            p.Codigos = Codigos;

            return p;
        }
        public virtual string[] GetColunas()
        {
            return new string[] { "Cód. de Barras", "Descrição", "Preço", "Fornecedor", "Marca", "Cód. De Barras de Fornecedor" };
        }

        public virtual object GetId()
        {
            return Cod_Barra;
        }
    }
}

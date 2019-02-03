using System.Collections.Generic;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.Model
{
    public class Produto : ObservableObject
    {
        private string cod_barra;
        private Fornecedor fornecedor;
        private Marca marca;
        private string descricao;
        private double preco;
        private IList<string> codigos = new List<string>();

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
        public virtual Fornecedor Fornecedor
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

        public virtual Marca Marca
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
                descricao = value;
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
    }
}

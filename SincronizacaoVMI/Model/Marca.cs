using NHibernate;
using System;
using System.Collections.Generic;
using ProdutoModel = SincronizacaoVMI.Model.Produto;

namespace SincronizacaoVMI.Model
{
    public class Marca : AModel, IModel
    {
        private string _nome;
        private Fornecedor _fornecedor;
        private IList<ProdutoModel> _produtos = new List<ProdutoModel>();

        public virtual string Nome
        {
            get => _nome;
            set
            {
                _nome = value.ToUpper();
                OnPropertyChanged("Nome");
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

        public virtual Fornecedor Fornecedor
        {
            get => _fornecedor;

            set
            {
                _fornecedor = value;
                OnPropertyChanged("Fornecedor");
            }
        }

        public virtual object GetIdentifier()
        {
            return Nome;
        }

        public virtual void Copiar(object source)
        {
            throw new NotImplementedException();
        }
    }
}

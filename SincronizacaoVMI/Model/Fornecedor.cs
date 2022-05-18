using System.Collections.Generic;
using System.Text.RegularExpressions;
using ProdutoModel = SincronizacaoVMI.Model.Produto;

namespace SincronizacaoVMI.Model
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

        public virtual IList<ProdutoModel> Produtos
        {
            get => _produtos;
            set
            {
                _produtos = value;
                OnPropertyChanged("Produtos");
            }
        }

        public virtual Representante Representante
        {
            get => _representante;
            set
            {
                _representante = value;
                OnPropertyChanged("Representante");
            }
        }
        public virtual object GetIdentifier()
        {
            return Cnpj;
        }
    }
}

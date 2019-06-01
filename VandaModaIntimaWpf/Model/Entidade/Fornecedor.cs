using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Fornecedor : ObservableObject
    {
        public virtual string cnpj { get; set; }
        public virtual string nome { get; set; }
        public virtual string nomeFantasia { get; set; }
        public virtual string email { get; set; }

        public virtual IList<Produto> Produtos { get; set; } = new List<Produto>();

        public Fornecedor() { }
        public Fornecedor(string Cnpj, string Nome, string NomeFantasia, string Email)
        {
            cnpj = Cnpj;
            nome = Nome;
            nomeFantasia = NomeFantasia;
            email = Email;
        }

        public virtual string Cnpj
        {
            get { return cnpj; }
            set
            {
                cnpj = value;
                OnPropertyChanged("Cnpj");
            }
        }

        public virtual string Nome
        {
            get { return nome; }
            set
            {
                nome = value;
                OnPropertyChanged("Nome");
            }
        }

        public virtual string NomeFantasia
        {
            get { return nomeFantasia; }
            set
            {
                nomeFantasia = value;
                OnPropertyChanged("NomeFantasia");
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
    }
}

using System;

namespace VandaModaIntimaWpf.Model
{
    public class Loja : ObservableObject, ICloneable, IModel
    {
        private string cnpj { get; set; }
        private Loja matriz { get; set; }
        private string nome { get; set; }
        private string telefone { get; set; }
        private string endereco { get; set; }
        private string inscricaoestadual { get; set; }
        private DateTime lastUpdate { get; set; } = DateTime.Now;
        public Loja() { }
        public Loja(string nome)
        {
            this.nome = nome;
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
        public virtual Loja Matriz
        {
            get { return matriz; }
            set
            {
                matriz = value;
                OnPropertyChanged("Matriz");
            }
        }
        public virtual string Nome
        {
            get { return nome?.ToUpper(); }
            set
            {
                nome = value;
                OnPropertyChanged("Nome");
            }
        }
        public virtual string Telefone
        {
            get { return telefone; }
            set
            {
                telefone = value;
                OnPropertyChanged("Telefone");
            }
        }
        public virtual string Endereco
        {
            get { return endereco?.ToUpper(); }
            set
            {
                endereco = value;
                OnPropertyChanged("Endereco");
            }
        }
        public virtual string InscricaoEstadual
        {
            get { return inscricaoestadual; }
            set
            {
                inscricaoestadual = value;
                OnPropertyChanged("InscricaoEstadual");
            }
        }

        public virtual DateTime LastUpdate
        {
            get { return lastUpdate; }
            set
            {
                lastUpdate = value;
                OnPropertyChanged("LastUpdate");
            }
        }

        public virtual string GetContextMenuHeader { get => Nome; }

        public virtual object Clone()
        {
            Loja loja = new Loja();

            loja.Cnpj = Cnpj;
            loja.Nome = Nome;
            loja.Telefone = Telefone;
            loja.Endereco = Endereco;
            loja.InscricaoEstadual = InscricaoEstadual;
            loja.Matriz = Matriz;

            return loja;
        }

        public virtual object GetId()
        {
            return Cnpj;
        }
    }
}

using Newtonsoft.Json;
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
        public Loja() { }
        public Loja(string nome)
        {
            this.nome = nome;
        }
        public string Cnpj
        {
            get { return cnpj; }
            set
            {
                cnpj = value;
                OnPropertyChanged("Cnpj");
            }
        }
        public Loja Matriz
        {
            get { return matriz; }
            set
            {
                matriz = value;
                OnPropertyChanged("Matriz");
            }
        }
        public string Nome
        {
            get { return nome?.ToUpper(); }
            set
            {
                nome = value;
                OnPropertyChanged("Nome");
            }
        }
        public string Telefone
        {
            get { return telefone; }
            set
            {
                telefone = value;
                OnPropertyChanged("Telefone");
            }
        }
        public string Endereco
        {
            get { return endereco?.ToUpper(); }
            set
            {
                endereco = value;
                OnPropertyChanged("Endereco");
            }
        }
        public string InscricaoEstadual
        {
            get { return inscricaoestadual; }
            set
            {
                inscricaoestadual = value;
                OnPropertyChanged("InscricaoEstadual");
            }
        }

        [JsonIgnore]
        public string GetContextMenuHeader { get => Nome; }

        public object Clone()
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

        public object GetIdentifier()
        {
            return Cnpj;
        }

        public string GetDatabaseLogIdentifier()
        {
            return Cnpj;
        }
    }
}

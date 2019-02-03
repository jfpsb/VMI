using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Marca : ObservableObject
    {
        private long id { get; set; }
        private string nome { get; set; }

        public virtual IList<Produto> Produtos { get; set; } = new List<Produto>();

        public Marca() { }
        public Marca(long Id, string Nome)
        {
            id = Id;
            nome = Nome;
        }

        public virtual long Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model
{
    class Loja : ObservableObject, ICloneable, IModel
    {
        private string cnpj { get; set; }
        private Loja matriz { get; set; }
        private string nome { get; set; }
        private string telefone { get; set; }
        private string endereco { get; set; }
        private string inscricaoestadual { get; set; }
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
            get { return nome; }
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
            get { return endereco; }
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
        public virtual object Clone()
        {
            throw new NotImplementedException();
        }

        public virtual object GetId()
        {
            return Cnpj;
        }
    }
}

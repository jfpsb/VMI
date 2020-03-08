using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model
{
    class TipoContagem : ObservableObject, ICloneable, IModel
    {
        private int id;
        private string nome;

        [JsonIgnore]
        public virtual string GetContextMenuHeader { get { return Nome; } }

        public virtual int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public virtual string Nome
        {
            get
            {
                return nome;
            }

            set
            {
                nome = value;
                OnPropertyChanged("Nome");
            }
        }

        public virtual object Clone()
        {
            throw new NotImplementedException();
        }

        public virtual object GetIdentifier()
        {
            return Id.ToString();
        }
    }
}

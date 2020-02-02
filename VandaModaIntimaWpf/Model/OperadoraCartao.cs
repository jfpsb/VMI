using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace VandaModaIntimaWpf.Model
{
    public class OperadoraCartao : ObservableObject, ICloneable, IModel
    {
        private string nome;
        private IList<string> identificadoresBanco = new List<string>();
        public virtual string Nome
        {
            get { return nome; }
            set
            {
                nome = value;
                OnPropertyChanged("Nome");
            }
        }
        [XmlArray("Identificadores")]
        [XmlArrayItem("Identificador")]
        public virtual IList<string> IdentificadoresBanco
        {
            get { return identificadoresBanco; }
            set
            {
                identificadoresBanco = value;
                OnPropertyChanged("IdentificadoresBanco");
            }
        }
        public virtual string[] IdentificadoresBancoArray
        {
            get { return IdentificadoresBanco.ToArray(); }
        }

        public virtual string GetContextMenuHeader { get => Nome; }

        public virtual object Clone()
        {
            throw new NotImplementedException();
        }

        public virtual object GetId()
        {
            return Nome;
        }
    }
}

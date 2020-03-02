using System.Collections.Generic;

namespace SincronizacaoBD.Model
{
    public class OperadoraCartao : IModel
    {
        private string nome;
        private IList<string> identificadoresBanco = new List<string>();
        public virtual string Nome
        {
            get { return nome; }
            set
            {
                nome = value;
            }
        }

        public virtual IList<string> IdentificadoresBanco
        {
            get { return identificadoresBanco; }
            set
            {
                identificadoresBanco = value;
            }
        }

        public virtual object GetIdentifier()
        {
            return Nome;
        }
    }
}

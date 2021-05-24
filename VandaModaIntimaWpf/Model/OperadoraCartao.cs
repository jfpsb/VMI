using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VandaModaIntimaWpf.Model
{
    public class OperadoraCartao : AModel, IModel
    {
        private string _nome;
        private IList<string> _identificadoresBanco = new List<string>();

        [JsonIgnore]
        public virtual Dictionary<string, string> DictionaryIdentifier
        {
            get
            {
                var dic = new Dictionary<string, string>
                {
                    { "Nome", Nome }
                };

                return dic;
            }
        }

        public virtual string Nome
        {
            get => _nome;
            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
            }
        }

        public virtual IList<string> IdentificadoresBanco
        {
            get => _identificadoresBanco;
            set
            {
                _identificadoresBanco = value;
                OnPropertyChanged("IdentificadoresBanco");
            }
        }

        public virtual bool IsIdentical(object obj)
        {
            if (obj != null && obj.GetType() == typeof(OperadoraCartao))
            {
                OperadoraCartao operadoraCartao = (OperadoraCartao)obj;
                return operadoraCartao.Nome.Equals(Nome)
                       && operadoraCartao.IdentificadoresBanco.SequenceEqual(IdentificadoresBanco);
            }

            return false;
        }

        [JsonIgnore]
        public virtual string GetContextMenuHeader => Nome;

        public virtual object GetIdentifier()
        {
            return Nome;
        }

        public virtual void InicializaLazyLoad()
        {
            throw new NotImplementedException("OperadoraCartao Não Possui Propriedades Que Usam Lazy Loading");
        }
    }
}

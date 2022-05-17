using System;
using System.Collections.Generic;

namespace SincronizacaoVMI.Model
{
    public class OperadoraCartao : AModel, IModel
    {
        private string _nome;
        private IList<string> _identificadoresBanco = new List<string>();

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

        public virtual void Copiar(object source)
        {
            throw new NotImplementedException();
        }

        public virtual object GetIdentifier()
        {
            return Nome;
        }
    }
}

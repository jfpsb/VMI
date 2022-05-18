using System;

namespace SincronizacaoVMI.Model
{
    public class OperadoraCartao : AModel, IModel
    {
        private string _nome;

        public virtual string Nome
        {
            get => _nome;
            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
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

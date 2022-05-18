using System;

namespace SincronizacaoVMI.Model
{
    public class TipoHoraExtra : AModel, IModel
    {
        private int _id;
        private string _descricao;
        public virtual int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual string Descricao
        {
            get => _descricao;
            set
            {
                _descricao = value;
                OnPropertyChanged("Descricao");
            }
        }
        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}

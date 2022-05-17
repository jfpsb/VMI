using System;
using System.Collections.Generic;

namespace SincronizacaoVMI.Model
{
    public class EntradaDeMercadoria : AModel, IModel
    {
        private int _id;
        private Loja _loja;
        private DateTime _data;
        private IList<EntradaMercadoriaProdutoGrade> _entradas = new List<EntradaMercadoriaProdutoGrade>();
        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual void Copiar(object source)
        {
            throw new NotImplementedException();
        }

        public virtual int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual Loja Loja
        {
            get => _loja;
            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
            }
        }
        public virtual DateTime Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }
        public virtual IList<EntradaMercadoriaProdutoGrade> Entradas
        {
            get => _entradas;
            set
            {
                _entradas = value;
                OnPropertyChanged("Entradas");
            }
        }
    }
}

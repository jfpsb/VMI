using System;

namespace VandaModaIntimaWpf.Model
{
    public class Duplicata : ObservableObject
    {
        private Type _tipoEntidade;
        private Adiantamento _entidadeRemota;
        private Adiantamento _entidadeLocal;

        public Type TipoEntidade
        {
            get
            {
                return _tipoEntidade;
            }

            set
            {
                _tipoEntidade = value;
                OnPropertyChanged("TipoEntidade");
            }
        }

        public Adiantamento EntidadeRemota
        {
            get
            {
                return _entidadeRemota;
            }

            set
            {
                _entidadeRemota = value;
                OnPropertyChanged("EntidadeRemota");
            }
        }

        public Adiantamento EntidadeLocal
        {
            get
            {
                return _entidadeLocal;
            }

            set
            {
                _entidadeLocal = value;
                OnPropertyChanged("EntidadeLocal");
            }
        }
    }
}

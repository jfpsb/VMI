using System;

namespace SincronizacaoVMI.Model
{
    public class InscricaoImobiliaria : AModel, IModel
    {
        private int _id;
        private Loja _loja;
        private string _numeracao;
        private DateTime _inicioLocacao;
        private DateTime? _fimLocacao;

        public virtual int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public virtual Loja Loja
        {
            get
            {
                return _loja;
            }

            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
            }
        }

        public virtual string Numeracao
        {
            get
            {
                return _numeracao;
            }

            set
            {
                _numeracao = value;
                OnPropertyChanged("Numeracao");
            }
        }

        public virtual DateTime InicioLocacao
        {
            get
            {
                return _inicioLocacao;
            }

            set
            {
                _inicioLocacao = value;
                OnPropertyChanged("InicioLocacao");
            }
        }

        public virtual DateTime? FimLocacao
        {
            get
            {
                return _fimLocacao;
            }

            set
            {
                _fimLocacao = value;
                OnPropertyChanged("FimLocacao");
            }
        }
        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}

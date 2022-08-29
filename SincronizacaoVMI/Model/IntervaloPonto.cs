using System;

namespace SincronizacaoVMI.Model
{
    public class IntervaloPonto : AModel, IModel
    {
        private int _id;
        private PontoEletronico _pontoEletronico;
        private DateTime _inicio;
        private DateTime? _fim;

        public virtual int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public virtual PontoEletronico PontoEletronico
        {
            get
            {
                return _pontoEletronico;
            }

            set
            {
                _pontoEletronico = value;
            }
        }

        public virtual DateTime Inicio
        {
            get
            {
                return _inicio;
            }

            set
            {
                _inicio = value;
            }
        }

        public virtual DateTime? Fim
        {
            get
            {
                return _fim;
            }

            set
            {
                _fim = value;
            }
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}

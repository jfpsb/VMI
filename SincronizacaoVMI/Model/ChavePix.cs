using System;

namespace SincronizacaoVMI.Model
{
    public class ChavePix : AModel, IModel
    {
        private int _id;
        private Funcionario _funcionario;
        private Banco _banco;
        private string _chave;

        public virtual int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual Funcionario Funcionario
        {
            get => _funcionario;
            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
            }
        }
        public virtual string Chave
        {
            get => _chave;
            set
            {
                _chave = value;
                OnPropertyChanged("Chave");
            }
        }

        public virtual Banco Banco
        {
            get => _banco;
            set
            {
                _banco = value;
                OnPropertyChanged("Banco");
            }
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}

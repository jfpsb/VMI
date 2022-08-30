using System;

namespace SincronizacaoVMI.Model
{
    public class BonusMensal : AModel, IModel
    {
        private int _id;
        private Funcionario _funcionario;
        private string _descricao;
        private double _valor;
        private bool _pagoEmFolha = true;

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
        public virtual string Descricao
        {
            get => _descricao;
            set
            {
                _descricao = value;
                OnPropertyChanged("Descricao");
            }
        }
        public virtual double Valor
        {
            get => _valor;
            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }

        public virtual bool PagoEmFolha
        {
            get
            {
                return _pagoEmFolha;
            }

            set
            {
                _pagoEmFolha = value;
                OnPropertyChanged("PagoEmFolha");
            }
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}

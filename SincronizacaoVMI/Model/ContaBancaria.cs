namespace SincronizacaoVMI.Model
{
    public class ContaBancaria : AModel, IModel
    {
        private int _id;
        private Funcionario _funcionario;
        private Banco _banco;
        private string _agencia;
        private string _operacao;
        private string _conta;
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
        public virtual Banco Banco
        {
            get => _banco;
            set
            {
                _banco = value;
                OnPropertyChanged("Banco");
            }
        }
        public virtual string Agencia
        {
            get => _agencia;
            set
            {
                _agencia = value;
                OnPropertyChanged("Agencia");
            }
        }
        public virtual string Operacao
        {
            get => _operacao;
            set
            {
                _operacao = value;
                OnPropertyChanged("Operacao");
            }
        }
        public virtual string Conta
        {
            get => _conta;
            set
            {
                _conta = value;
                OnPropertyChanged("Conta");
            }
        }
        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}

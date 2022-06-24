namespace SincronizacaoVMI.Model
{
    public class OperadoraCartaoId : AModel
    {
        private string _identificador;
        private OperadoraCartao _operadoraCartao;

        public virtual string Identificador
        {
            get
            {
                return _identificador;
            }

            set
            {
                _identificador = value;
            }
        }

        public virtual OperadoraCartao OperadoraCartao
        {
            get
            {
                return _operadoraCartao;
            }

            set
            {
                _operadoraCartao = value;
            }
        }
    }
}

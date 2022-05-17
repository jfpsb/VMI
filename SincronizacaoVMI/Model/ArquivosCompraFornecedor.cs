namespace SincronizacaoVMI.Model
{
    public class ArquivosCompraFornecedor : AModel, IModel
    {
        private int _id;
        private CompraDeFornecedor _compraDeFornecedor;
        private string _nome;
        private string _extensao;
        private string _caminhoOriginal;

        public virtual int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual CompraDeFornecedor CompraDeFornecedor
        {
            get => _compraDeFornecedor;
            set
            {
                _compraDeFornecedor = value;
                OnPropertyChanged("CompraDeFornecedor");
            }
        }
        public virtual string Nome
        {
            get => _nome;
            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
            }
        }
        public virtual string Extensao
        {
            get => _extensao;
            set
            {
                _extensao = value;
                OnPropertyChanged("Extensao");
            }
        }

        public virtual void Copiar(object source)
        {
            throw new System.NotImplementedException();
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}

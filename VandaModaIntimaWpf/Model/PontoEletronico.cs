using System;

namespace VandaModaIntimaWpf.Model
{
    public class PontoEletronico : AModel, IModel
    {
        private int _id;
        private Funcionario _funcionario;
        private DateTime _dia;
        private DateTime? _entrada;
        private DateTime? _saida;
        private DateTime? _entradaAlmoco;
        private DateTime? _saidaAlmoco;
        public string GetContextMenuHeader => throw new NotImplementedException();

        public int Id
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

        public Funcionario Funcionario
        {
            get
            {
                return _funcionario;
            }

            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
            }
        }

        public DateTime? Entrada
        {
            get
            {
                return _entrada;
            }

            set
            {
                _entrada = value;
                OnPropertyChanged("Entrada");
            }
        }

        public DateTime? Saida
        {
            get
            {
                return _saida;
            }

            set
            {
                _saida = value;
                OnPropertyChanged("Saida");
            }
        }

        public DateTime? EntradaAlmoco
        {
            get
            {
                return _entradaAlmoco;
            }

            set
            {
                _entradaAlmoco = value;
                OnPropertyChanged("EntradaAlmoco");
            }
        }

        public DateTime? SaidaAlmoco
        {
            get
            {
                return _saidaAlmoco;
            }

            set
            {
                _saidaAlmoco = value;
                OnPropertyChanged("SaidaAlmoco");
            }
        }

        public DateTime Dia
        {
            get
            {
                return _dia;
            }

            set
            {
                _dia = value;
                OnPropertyChanged("Dia");
            }
        }

        public object GetIdentifier()
        {
            return Id;
        }

        public void InicializaLazyLoad()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model
{
    public class ChavePix : AModel, IModel
    {
        private long _id;
        private Funcionario _funcionario;
        private Banco _banco;
        private string _chave;

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual long Id
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

        public virtual void InicializaLazyLoad()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

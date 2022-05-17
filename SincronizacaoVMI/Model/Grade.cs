using System;
using System.Collections.Generic;

namespace SincronizacaoVMI.Model
{
    public class Grade : AModel, IModel
    {
        private int _id;
        private TipoGrade _tipoGrade;
        private string _nome;
        private IList<ProdutoGrade> _produtoGrades;

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

        public virtual TipoGrade TipoGrade
        {
            get
            {
                return _tipoGrade;
            }

            set
            {
                _tipoGrade = value;
                OnPropertyChanged("TipoGrade");
            }
        }

        public virtual string Nome
        {
            get
            {
                return _nome?.ToUpper();
            }

            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
            }
        }

        public virtual IList<ProdutoGrade> ProdutoGrades
        {
            get => _produtoGrades;
            set
            {
                _produtoGrades = value;
                OnPropertyChanged("ProdutoGrades");
            }
        }

        public virtual void Copiar(object source)
        {
            throw new NotImplementedException();
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}

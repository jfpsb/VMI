using System;
using System.Collections.Generic;

namespace SincronizacaoVMI.Model
{
    public class SubGrade : AModel, IModel
    {
        private int _id;
        private ProdutoGrade _produtoGrade;
        private Grade _grade;

        public virtual int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual ProdutoGrade ProdutoGrade
        {
            get
            {
                return _produtoGrade;
            }

            set
            {
                _produtoGrade = value;
                OnPropertyChanged("ProdutoGrade");
            }
        }

        public virtual Grade Grade
        {
            get
            {
                return _grade;
            }

            set
            {
                _grade = value;
                OnPropertyChanged("Grade");
            }
        }
        public virtual void Copiar(object source)
        {
            throw new NotImplementedException();
        }

        public virtual object GetIdentifier()
        {
            return this;
        }
    }
}

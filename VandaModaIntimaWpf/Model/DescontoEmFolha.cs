using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;

namespace VandaModaIntimaWpf.Model
{
    public class DescontoEmFolha : AModel, IModel
    {
        private int _id;
        private Funcionario _funcionario;
        private string _descricao;
        private int _mesReferencia;
        private int _anoReferencia;
        private bool _repeteMensal;
        private double _valor;

        public virtual string GetContextMenuHeader => Descricao;

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

        public virtual Funcionario Funcionario
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

        public virtual string Descricao
        {
            get
            {
                return _descricao;
            }

            set
            {
                _descricao = value;

                if (Valor != 0.0)
                {
                    _descricao += $" = {Valor}";
                }

                OnPropertyChanged("Descricao");
            }
        }

        public virtual int MesReferencia
        {
            get
            {
                return _mesReferencia;
            }

            set
            {
                _mesReferencia = value;
                OnPropertyChanged("MesReferencia");
            }
        }

        public virtual int AnoReferencia
        {
            get
            {
                return _anoReferencia;
            }

            set
            {
                _anoReferencia = value;
                OnPropertyChanged("AnoReferencia");
            }
        }

        public virtual bool RepeteMensal
        {
            get
            {
                return _repeteMensal;
            }

            set
            {
                _repeteMensal = value;
                OnPropertyChanged("RepeteMensal");
            }
        }

        public virtual double Valor
        {
            get
            {
                return _valor;
            }

            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }

        public virtual object GetIdentifier()
        {
            throw new NotImplementedException();
        }

        public virtual void InicializaLazyLoad()
        {
            throw new NotImplementedException();
        }
    }
}

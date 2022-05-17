using System;
using System.Collections.Generic;

namespace SincronizacaoVMI.Model
{
    public class Adiantamento : AModel, IModel
    {
        private int _id;
        private Funcionario _funcionario;
        private DateTime _data;
        private double _valor;
        private string _descricao;
        private IList<Parcela> _parcelas = new List<Parcela>();

        public virtual string GetContextMenuHeader => _data.ToString("d") + " - " + _funcionario.Nome;

        public virtual DateTime Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }

        public virtual string DataString
        {
            get => _data.ToString("G");
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

        public virtual Funcionario Funcionario
        {
            get => _funcionario;
            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
            }
        }

        public virtual IList<Parcela> Parcelas
        {
            get => _parcelas;
            set
            {
                _parcelas = value;
                OnPropertyChanged("Parcelas");
            }
        }

        public virtual string Descricao
        {
            get => _descricao;
            set
            {
                _descricao = value?.ToUpper();
                OnPropertyChanged("Descricao");
            }
        }

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

        public virtual void Copiar(object source)
        {
            Adiantamento a = source as Adiantamento;
            Funcionario = a.Funcionario;
            Data = a.Data;
            Valor = a.Valor;
            Descricao = a.Descricao;
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}

using System;
using FornecedorModel = SincronizacaoVMI.Model.Fornecedor;
using MarcaModel = SincronizacaoVMI.Model.Marca;

namespace SincronizacaoVMI.Model
{
    public class Produto : AModel, IModel
    {
        private int _id;
        private string _codBarra;
        private FornecedorModel _fornecedor;
        private MarcaModel _marca;
        private string _descricao;
        private string _ncm;

        public virtual int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual string CodBarra
        {
            get => _codBarra;
            set
            {
                if (!value.Equals(_codBarra))
                {
                    _codBarra = value;
                    OnPropertyChanged("CodBarra");
                }
            }
        }
        public virtual FornecedorModel Fornecedor
        {
            get => _fornecedor;

            set
            {
                _fornecedor = value;
                OnPropertyChanged("Fornecedor");
            }
        }

        public virtual MarcaModel Marca
        {
            get => _marca;
            set
            {
                _marca = value;
                OnPropertyChanged("Marca");
            }
        }

        public virtual string Descricao
        {
            get => _descricao;

            set
            {
                _descricao = value.ToUpper();
                OnPropertyChanged("Descricao");
            }
        }

        public virtual string Ncm
        {
            get => _ncm;
            set
            {
                _ncm = value;
                OnPropertyChanged("Ncm");
            }
        }
        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}

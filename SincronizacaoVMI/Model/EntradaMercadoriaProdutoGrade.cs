﻿namespace SincronizacaoVMI.Model
{
    public class EntradaMercadoriaProdutoGrade : AModel, IModel
    {
        private int _id;
        private EntradaDeMercadoria _entrada;
        private ProdutoGrade _produtoGrade;
        private int _quantidade;

        private string _produtoDescricao;
        private string _gradeDescricao;
        private double _gradePreco;
        private Fornecedor _gradeFornecedor;

        public virtual object GetIdentifier()
        {
            return Id;
        }
        public virtual int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual EntradaDeMercadoria Entrada
        {
            get => _entrada;
            set
            {
                _entrada = value;
                OnPropertyChanged("Entrada");
            }
        }
        public virtual ProdutoGrade ProdutoGrade
        {
            get => _produtoGrade;
            set
            {
                _produtoGrade = value;
                OnPropertyChanged("ProdutoGrade");
            }
        }
        public virtual int Quantidade
        {
            get => _quantidade;
            set
            {
                _quantidade = value;
                OnPropertyChanged("Quantidade");
            }
        }

        public virtual string ProdutoDescricao
        {
            get => _produtoDescricao;
            set
            {
                _produtoDescricao = value;
                OnPropertyChanged("ProdutoDescricao");
            }
        }
        public virtual string GradeDescricao
        {
            get => _gradeDescricao;
            set
            {
                _gradeDescricao = value;
                OnPropertyChanged("GradeDescricao");
            }
        }
        public virtual double GradePreco
        {
            get => _gradePreco;
            set
            {
                _gradePreco = value;
                OnPropertyChanged("GradePreco");
            }
        }

        public virtual Fornecedor GradeFornecedor
        {
            get => _gradeFornecedor;
            set
            {
                _gradeFornecedor = value;
                OnPropertyChanged("GradeFornecedor");
            }
        }
    }
}

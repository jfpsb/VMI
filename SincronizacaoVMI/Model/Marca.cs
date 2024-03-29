﻿using NHibernate;
using System;
using System.Collections.Generic;
using ProdutoModel = SincronizacaoVMI.Model.Produto;

namespace SincronizacaoVMI.Model
{
    public class Marca : AModel, IModel
    {
        private string _nome;
        private Fornecedor _fornecedor;

        public virtual string Nome
        {
            get => _nome;
            set
            {
                _nome = value.ToUpper();
                OnPropertyChanged("Nome");
            }
        }
        public virtual Fornecedor Fornecedor
        {
            get => _fornecedor;

            set
            {
                _fornecedor = value;
                OnPropertyChanged("Fornecedor");
            }
        }

        public virtual object GetIdentifier()
        {
            return Nome;
        }
    }
}

﻿using NHibernate;
using System;
using System.Windows;
using VandaModaIntimaWpf.Resources;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class EditarProdutoViewModel : CadastrarProdutoViewModel
    {
        public EditarProdutoViewModel(ISession session) : base(session)
        {

        }

        public override async void Salvar(object parameter)
        {
            if (Produto.Fornecedor?.Cnpj == null)
                Produto.Fornecedor = null;

            if (Produto.Marca != null && Produto.Marca.Nome.Equals(GetResource.GetString("marca_nao_selecionada")))
                Produto.Marca = null;

            _result = await daoProduto.Merge(Produto);

            AposCadastrarEventArgs e = new AposCadastrarEventArgs()
            {
                SalvoComSucesso = _result,
                MensagemSucesso = $"Produto {Produto.CodBarra} Atualizado Com Sucesso",
                MensagemErro = GetResource.GetString("erro_ao_atualizar_produto"),
                ObjetoSalvo = Produto
            };

            ChamaAposCadastrar(e);
        }

        public new ProdutoModel Produto
        {
            get { return produtoModel; }
            set
            {
                produtoModel = value;
                FornecedorComboBox = value.Fornecedor;
                MarcaComboBox = value.Marca;
                OnPropertyChanged("Produto");
                //OnPropertyChanged("FornecedorComboBox");
                //OnPropertyChanged("MarcaComboBox");
            }
        }

        public FornecedorModel FornecedorComboBox
        {
            get
            {
                if (Produto.Fornecedor == null)
                {
                    Produto.Fornecedor = Fornecedores[0];
                }

                return Produto.Fornecedor;
            }

            set
            {
                Produto.Fornecedor = value;
                OnPropertyChanged("FornecedorComboBox");
            }
        }

        public MarcaModel MarcaComboBox
        {
            get
            {
                if (Produto.Marca == null)
                {
                    Produto.Marca = Marcas[0];
                }

                return Produto.Marca;
            }

            set
            {
                Produto.Marca = value;
                OnPropertyChanged("MarcaComboBox");
            }
        }
    }
}

﻿using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class CadastrarProdutoViewModel : ACadastrarViewModel
    {
        protected DAOProduto daoProduto;
        protected DAOMarca daoMarca;
        protected DAOFornecedor daoFornecedor;
        protected ProdutoModel produtoModel;
        private int fornecedorComboBoxIndex = 0;
        private int marcaComboBoxIndex = 0;
        public ObservableCollection<FornecedorModel> Fornecedores { get; set; }
        public ObservableCollection<MarcaModel> Marcas { get; set; }
        public CadastrarProdutoViewModel(ISession session)
        {
            _session = session;
            daoProduto = new DAOProduto(_session);
            daoMarca = new DAOMarca(_session);
            daoFornecedor = new DAOFornecedor(_session);
            produtoModel = new ProdutoModel();

            produtoModel.PropertyChanged += CadastrarViewModel_PropertyChanged;

            GetFornecedores();
            GetMarcas();

            int novoCodBarra = daoProduto.GetMaxId();
            Produto.CodBarra = (novoCodBarra + 1).ToString();
        }
        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Produto.CodBarra) || string.IsNullOrEmpty(Produto.Descricao))
            {
                return false;
            }

            if (Produto.Preco.ToString().Equals(string.Empty) || Produto.Preco == 0)
            {
                return false;
            }

            if (!IsEnabled)
                return false;

            return true;
        }

        public override async void Salvar(object parameter)
        {
            if (FornecedorComboBoxIndex == 0)
                Produto.Fornecedor = null;

            if (MarcaComboBoxIndex == 0)
                Produto.Marca = null;

            _result = await daoProduto.Inserir(produtoModel);

            if (_result)
            {
                ResetaPropriedades();
                await SetStatusBarSucesso("Produto Cadastrado Com Sucesso");
                return;
            }

            SetStatusBarErro("Erro ao Cadastrar Produto");
        }

        public override void ResetaPropriedades()
        {
            Produto = new ProdutoModel();
            Produto.CodBarra = Produto.Descricao = string.Empty;
            Produto.Preco = 0;
            Produto.Fornecedor = Fornecedores[0];
            Produto.Marca = Marcas[0];
        }

        public ProdutoModel Produto
        {
            get { return produtoModel; }
            set
            {
                produtoModel = value;
                OnPropertyChanged("Produto");
            }
        }

        private async void GetFornecedores()
        {
            Fornecedores = new ObservableCollection<FornecedorModel>(await daoFornecedor.Listar<FornecedorModel>());
            Fornecedores.Insert(0, new FornecedorModel(StringResource.GetString("fornecedor_nao_selecionado")));
        }

        private async void GetMarcas()
        {
            Marcas = new ObservableCollection<MarcaModel>(await daoMarca.Listar<MarcaModel>());
            Marcas.Insert(0, new MarcaModel(StringResource.GetString("marca_nao_selecionada")));
        }

        public override async void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CodBarra":
                    var result = await daoProduto.ListarPorId(Produto.CodBarra);

                    if (result != null)
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Visible;
                        IsEnabled = false;
                    }
                    else
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Collapsed;
                        IsEnabled = true;
                    }

                    break;
            }
        }
        public int FornecedorComboBoxIndex
        {
            get { return fornecedorComboBoxIndex; }
            set
            {
                fornecedorComboBoxIndex = value;
                OnPropertyChanged("FornecedorComboBoxIndex");
            }
        }
        public int MarcaComboBoxIndex
        {
            get { return marcaComboBoxIndex; }
            set
            {
                marcaComboBoxIndex = value;
                OnPropertyChanged("MarcaComboBoxIndex");
            }
        }
    }
}

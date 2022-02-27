using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria
{
    class CadastrarEntradaDeMercadoriaVM : ACadastrarViewModel<Model.EntradaDeMercadoria>
    {
        private string _termoPesquisaProduto;
        private DAOProduto daoProduto;
        private Model.Produto _produto;
        private int _listViewZIndex;
        private IList<Model.Produto> _produtos = new List<Model.Produto>();
        public CadastrarEntradaDeMercadoriaVM(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            viewModelStrategy = new CadastrarEntradaDeMercadoriaVMStrategy();
            daoEntidade = new DAOEntradaDeMercadoria(session);
            daoProduto = new DAOProduto(session);
            Entidade = new Model.EntradaDeMercadoria();

            PropertyChanged += PesquisaProdutos;
        }

        private async void PesquisaProdutos(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TermoPesquisaProduto"))
            {
                if (TermoPesquisaProduto.Length > 0)
                {
                    ListViewZIndex = 1;
                }
                else
                {
                    ListViewZIndex = 0;
                }
                //Produtos = await daoProduto.ListarPorDescricaoCodigoDeBarra(TermoPesquisaProduto);
            }
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        public override void ResetaPropriedades(AposInserirBDEventArgs e)
        {
            Entidade = new Model.EntradaDeMercadoria();
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (Entidade.Entradas.Count == 0)
            {
                BtnSalvarToolTip += "Ao Menos Um Produto Deve Ser Adicionado Para Cadastrar A Entrada De Mercadoria!\n";
                valido = false;
            }

            return valido;
        }

        public string TermoPesquisaProduto
        {
            get => _termoPesquisaProduto;
            set
            {
                _termoPesquisaProduto = value;
                OnPropertyChanged("TermoPesquisaProduto");
            }
        }

        public IList<Model.Produto> Produtos
        {
            get => _produtos;
            set
            {
                _produtos = value;
                OnPropertyChanged("Produtos");
            }
        }

        public Model.Produto Produto
        {
            get => _produto;
            set
            {
                _produto = value;
                OnPropertyChanged("Produto");
            }
        }

        public int ListViewZIndex
        {
            get => _listViewZIndex;
            set
            {
                _listViewZIndex = value;
                OnPropertyChanged("ListViewZIndex");
            }
        }
    }
}

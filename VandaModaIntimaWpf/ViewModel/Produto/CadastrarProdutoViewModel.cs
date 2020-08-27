using NHibernate;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class CadastrarProdutoViewModel : ACadastrarViewModel<ProdutoModel>
    {
        protected DAOMarca daoMarca;
        protected DAOFornecedor daoFornecedor;

        private string _codigoFornecedor;

        public ObservableCollection<FornecedorModel> Fornecedores { get; set; }
        public ObservableCollection<MarcaModel> Marcas { get; set; }
        public ObservableCollection<string> CodigosFornecedor { get; set; }
        public ICommand InserirCodigoComando { get; set; }
        public CadastrarProdutoViewModel(ISession session) : base(session)
        {
            cadastrarViewModelStrategy = new CadastrarProdutoViewModelStrategy();
            daoEntidade = new DAOProduto(_session);
            daoMarca = new DAOMarca(_session);
            daoFornecedor = new DAOFornecedor(_session);
            Entidade = new ProdutoModel();

            Entidade.PropertyChanged += CadastrarViewModel_PropertyChanged;
            InserirCodigoComando = new RelayCommand(InserirCodigo, ValidaCodigoFornecedor);

            GetFornecedores();
            GetMarcas();
            CodigosFornecedor = new ObservableCollection<string>();

            AtribuiNovoCodBarra();
        }
        private bool ValidaCodigoFornecedor(object arg)
        {
            return CodigoFornecedor != null && !(CodigoFornecedor.Length == 0);
        }
        private void InserirCodigo(object obj)
        {
            if (!CodigosFornecedor.Contains(CodigoFornecedor))
            {
                CodigosFornecedor.Add(CodigoFornecedor);
                CodigoFornecedor = string.Empty;
                SetStatusBarAguardando();
            }
            else
            {
                SetStatusBarErro("Código Já Está Inserido");
            }
        }
        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Entidade.CodBarra) || string.IsNullOrEmpty(Entidade.Descricao))
            {
                return false;
            }

            if (Entidade.Preco.ToString().Equals(string.Empty) || Entidade.Preco == 0)
            {
                return false;
            }

            if (!IsEnabled)
                return false;

            return true;
        }
        public override void ResetaPropriedades()
        {
            Entidade = new ProdutoModel();
            Entidade.CodBarra = Entidade.Descricao = string.Empty;
            Entidade.Preco = 0;
            Entidade.Fornecedor = Fornecedores[0];
            Entidade.Marca = Marcas[0];

            AtribuiNovoCodBarra();
        }
        private void AtribuiNovoCodBarra()
        {
            int novoCodBarra = daoEntidade.GetMaxId();
            Entidade.CodBarra = (novoCodBarra + 1).ToString();
        }
        public string CodigoFornecedor
        {
            get
            {
                return _codigoFornecedor;
            }

            set
            {
                _codigoFornecedor = value;
                OnPropertyChanged("CodigoFornecedor");
            }
        }
        private async void GetFornecedores()
        {
            Fornecedores = new ObservableCollection<FornecedorModel>(await daoFornecedor.Listar<FornecedorModel>());
            Fornecedores.Insert(0, new FornecedorModel(GetResource.GetString("fornecedor_nao_selecionado")));
        }
        private async void GetMarcas()
        {
            Marcas = new ObservableCollection<MarcaModel>(await daoMarca.Listar<MarcaModel>());
            Marcas.Insert(0, new MarcaModel(GetResource.GetString("marca_nao_selecionada")));
        }
        public override async void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CodBarra":
                    var result = await daoEntidade.ListarPorId(Entidade.CodBarra);

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
        protected override void ExecutarAntesCriarDocumento()
        {
            if (Entidade.Fornecedor?.Cnpj == null)
                Entidade.Fornecedor = null;

            if (Entidade.Marca != null && Entidade.Marca.Nome.Equals(GetResource.GetString("marca_nao_selecionada")))
                Entidade.Marca = null;

            Entidade.Codigos = CodigosFornecedor;
        }
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto.Produto;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor.Fornecedor;
using MarcaModel = VandaModaIntimaWpf.Model.Marca.Marca;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    class CadastrarProdutoViewModel : ObservableObject, ICadastrarViewModel
    {
        private ProdutoModel produtoModel;
        private FornecedorModel fornecedorModel;
        private MarcaModel marcaModel;
        private Visibility visibilidadeAvisoCodBarra = Visibility.Collapsed;
        private bool isEnabled = true;
        private string mensagemStatusBar = "Aguardando Usuário";
        private string imagemStatusBar = IMAGEMAGUARDANDO;

        private static readonly string IMAGEMSUCESSO = "/Resources/Sucesso.png";
        private static readonly string IMAGEMERRO = "/Resources/Erro.png";
        private static readonly string IMAGEMAGUARDANDO = "/Resources/Aguardando.png";

        public ObservableCollection<FornecedorModel> Fornecedores { get; set; }
        public ObservableCollection<MarcaModel> Marcas { get; set; }

        public ICommand Command { get; set; }

        public bool ValidaModel(object parameter)
        {
            if (string.IsNullOrEmpty(Cod_Barra) || string.IsNullOrEmpty(Descricao))
            {
                return false;
            }

            if (Preco == 0)
            {
                return false;
            }

            return true;
        }

        public void Cadastrar(object parameter)
        {
            if (Fornecedor.Cnpj == null)
                Fornecedor = null;

            if (Marca.Id == 0)
                Marca = null;

            var result = produtoModel.Salvar();

            if (result)
            {
                MensagemStatusBar = "Cadastro Realizado Com Sucesso";
                ImagemStatusBar = IMAGEMSUCESSO;
                ResetaPropriedades();
                return;
            }

            MensagemStatusBar = "Erro ao Cadastrar";
            ImagemStatusBar = IMAGEMERRO;
        }

        public void ResetaPropriedades()
        {
            produtoModel = new ProdutoModel();
            Cod_Barra = "";
            Descricao = "";
            Preco = 0;
            Fornecedor = Fornecedores[0];
            Marca = Marcas[0];
        }

        public CadastrarProdutoViewModel()
        {
            produtoModel = new ProdutoModel();
            fornecedorModel = new FornecedorModel();
            marcaModel = new MarcaModel();

            Command = new RelayCommand(Cadastrar, ValidaModel);

            PropertyChanged += CadastrarProdutoViewModel_PropertyChanged;

            GetFornecedores();
            GetMarcas();
        }

        public string Cod_Barra
        {
            get
            {
                return produtoModel.Cod_Barra;
            }

            set
            {
                produtoModel.Cod_Barra = value.ToUpper();
                OnPropertyChanged("Cod_Barra");
            }
        }

        public string Descricao
        {
            get { return produtoModel.Descricao; }
            set
            {
                produtoModel.Descricao = value.ToUpper();
                OnPropertyChanged("Descricao");
            }
        }

        public double Preco
        {
            get { return produtoModel.Preco; }
            set
            {
                produtoModel.Preco = value;
                OnPropertyChanged("Preco");
            }
        }

        public FornecedorModel Fornecedor
        {
            get { return produtoModel.Fornecedor; }
            set
            {
                produtoModel.Fornecedor = value;
                OnPropertyChanged("Fornecedor");
            }
        }

        public MarcaModel Marca
        {
            get { return produtoModel.Marca; }
            set
            {
                produtoModel.Marca = value;
                OnPropertyChanged("Marca");
            }
        }

        public Visibility VisibilidadeAvisoCodBarra
        {
            get { return visibilidadeAvisoCodBarra; }
            set
            {
                visibilidadeAvisoCodBarra = value;
                OnPropertyChanged("VisibilidadeAvisoCodBarra");
            }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        public string MensagemStatusBar
        {
            get { return mensagemStatusBar; }
            set
            {
                mensagemStatusBar = value;
                OnPropertyChanged("MensagemStatusBar");
            }
        }

        public string ImagemStatusBar
        {
            get { return imagemStatusBar; }
            set
            {
                imagemStatusBar = value;
                OnPropertyChanged("ImagemStatusBar");
            }
        }

        private void GetFornecedores()
        {
            Fornecedores = new ObservableCollection<FornecedorModel>(fornecedorModel.Listar());
            Fornecedores.Insert(0, new FornecedorModel("SELECIONE O FORNECEDOR"));
        }

        private void GetMarcas()
        {
            Marcas = new ObservableCollection<MarcaModel>(marcaModel.Listar());
            Marcas.Insert(0, new MarcaModel("SELECIONE A MARCA"));
        }

        private void CadastrarProdutoViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cod_Barra":
                    var result = produtoModel.ListarPorId(Cod_Barra);

                    if (result != null)
                    {
                        VisibilidadeAvisoCodBarra = Visibility.Visible;
                        IsEnabled = false;
                    }
                    else
                    {
                        VisibilidadeAvisoCodBarra = Visibility.Collapsed;
                        IsEnabled = true;
                    }

                    break;
            }
        }

        public void DisposeServico()
        {
            produtoModel.Dispose();
        }
    }
}

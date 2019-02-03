using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.Servico;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    class CadastrarProdutoViewModel : ObservableObject, ICadastrarViewModel
    {
        private ProdutoServico produtoServico;
        private FornecedorServico fornecedorServico;
        private MarcaServico marcaServico;
        private Visibility visibilidadeAvisoCodBarra = Visibility.Collapsed;
        private bool isEnabled = true;
        private string mensagemStatusBar = "Aguardando Usuário";
        private string imagemStatusBar = IMAGEMAGUARDANDO;
        private ProdutoModel produto;

        private static readonly string IMAGEMSUCESSO = "/Resources/Sucesso.png";
        private static readonly string IMAGEMERRO = "/Resources/Erro.png";
        private static readonly string IMAGEMAGUARDANDO = "/Resources/Aguardando.png";

        public ObservableCollection<Fornecedor> Fornecedores { get; set; }
        public ObservableCollection<Marca> Marcas { get; set; }

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

            var result = produtoServico.Salvar(produto);

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
            produto = new ProdutoModel();
            Cod_Barra = "";
            Descricao = "";
            Preco = 0;
            Fornecedor = Fornecedores[0];
            Marca = Marcas[0];
        }

        public CadastrarProdutoViewModel()
        {
            produtoServico = new ProdutoServico();
            fornecedorServico = new FornecedorServico();
            marcaServico = new MarcaServico();
            produto = new ProdutoModel();

            Command = new RelayCommand(Cadastrar, ValidaModel);

            PropertyChanged += CadastrarProdutoViewModel_PropertyChanged;

            GetFornecedores();
            GetMarcas();
        }

        public string Cod_Barra
        {
            get
            {
                return produto.Cod_Barra;
            }

            set
            {
                produto.Cod_Barra = value.ToUpper();
                OnPropertyChanged("Cod_Barra");
            }
        }

        public string Descricao
        {
            get { return produto.Descricao; }
            set
            {
                produto.Descricao = value.ToUpper();
                OnPropertyChanged("Descricao");
            }
        }

        public double Preco
        {
            get { return produto.Preco; }
            set
            {
                produto.Preco = value;
                OnPropertyChanged("Preco");
            }
        }

        public Fornecedor Fornecedor
        {
            get { return produto.Fornecedor; }
            set
            {
                produto.Fornecedor = value;
                OnPropertyChanged("Fornecedor");
            }
        }

        public Marca Marca
        {
            get { return produto.Marca; }
            set
            {
                produto.Marca = value;
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
            Fornecedores = new ObservableCollection<Fornecedor>(fornecedorServico.Listar());
            Fornecedores.Insert(0, new Fornecedor(null, "SELECIONE O FORNECEDOR", null, null));
        }

        private void GetMarcas()
        {
            Marcas = new ObservableCollection<Marca>(marcaServico.Listar());
            Marcas.Insert(0, new Marca(0, "SELECIONE A MARCA"));
        }

        private void CadastrarProdutoViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cod_Barra":
                    var result = produtoServico.ListarPorId(Cod_Barra);

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
            produtoServico.DisposeDAO();
        }
    }
}

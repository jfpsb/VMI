using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor.Fornecedor;
using MarcaModel = VandaModaIntimaWpf.Model.Marca.Marca;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class CadastrarProdutoViewModel : ObservableObject, ICadastrarViewModel
    {
        protected ProdutoModel produtoModel;
        private FornecedorModel fornecedorModel;
        private MarcaModel marcaModel;
        private Visibility visibilidadeAvisoCodBarra = Visibility.Collapsed;
        protected bool isEnabled = true;
        private string mensagemStatusBar = "Aguardando Usuário";
        private string imagemStatusBar = IMAGEMAGUARDANDO;

        private static readonly string IMAGEMSUCESSO = "/Resources/Sucesso.png";
        private static readonly string IMAGEMERRO = "/Resources/Erro.png";
        private static readonly string IMAGEMAGUARDANDO = "/Resources/Aguardando.png";

        public ObservableCollection<FornecedorModel> Fornecedores { get; set; }
        public ObservableCollection<MarcaModel> Marcas { get; set; }
        public ICommand Command { get; set; }
        public CadastrarProdutoViewModel()
        {
            produtoModel = new ProdutoModel();
            fornecedorModel = new FornecedorModel();
            marcaModel = new MarcaModel();

            Command = new RelayCommand(Cadastrar, ValidaModel);

            produtoModel.PropertyChanged += CadastrarProdutoViewModel_PropertyChanged;

            GetFornecedores();
            GetMarcas();
        }
        public bool ValidaModel(object parameter)
        {
            if (string.IsNullOrEmpty(Produto.Cod_Barra) || string.IsNullOrEmpty(Produto.Descricao))
            {
                return false;
            }

            if (Produto.Preco.ToString().Equals(string.Empty) || Produto.Preco == 0)
            {
                return false;
            }

            return true;
        }

        public virtual void Cadastrar(object parameter)
        {
            if (Produto.Fornecedor.Cnpj.Equals("0"))
                Produto.Fornecedor = null;

            if (Produto.Marca.Id == 0)
                Produto.Marca = null;

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
            produtoModel.Fornecedor = Fornecedores[0];
            produtoModel.Marca = Marcas[0];
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
                    var result = produtoModel.ListarPorId(Produto.Cod_Barra);

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
    }
}

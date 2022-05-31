using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.View.Produto;
using VandaModaIntimaWpf.ViewModel.CompraDeFornecedor;
using VandaModaIntimaWpf.ViewModel.Contagem;
using VandaModaIntimaWpf.ViewModel.Despesa;
using VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria;
using VandaModaIntimaWpf.ViewModel.FolhaPagamento;
using VandaModaIntimaWpf.ViewModel.Fornecedor;
using VandaModaIntimaWpf.ViewModel.Funcionario;
using VandaModaIntimaWpf.ViewModel.Loja;
using VandaModaIntimaWpf.ViewModel.Marca;
using VandaModaIntimaWpf.ViewModel.Produto;
using VandaModaIntimaWpf.ViewModel.RecebimentoCartao;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel
{
    class VandaModaIntimaVM : ObservableObject
    {
        private IOpenViewService openView;
        public ICommand AbrirTelaProdutoComando { get; set; }
        public ICommand AbrirTelaFornecedorComando { get; set; }
        public ICommand AbrirTelaMarcaComando { get; set; }
        public ICommand AbrirTelaLojaComando { get; set; }
        public ICommand AbrirTelaRecebimentoComando { get; set; }
        public ICommand AbrirTelaContagemComando { get; set; }
        public ICommand AbrirTelaFolhaPagamentoComando { get; set; }
        public ICommand AbrirTelaFuncionarioComando { get; set; }
        public ICommand AbrirTelaDespesasComando { get; set; }
        public ICommand AbrirTelaCompraFornecedorComando { get; set; }
        public ICommand AbrirTelaEntradaMercadoriaComando { get; set; }
        public VandaModaIntimaVM()
        {
            openView = new OpenView();

            WindowService.RegistrarWindow<CadastrarProduto, CadastrarProdutoVM>();
            WindowService.RegistrarWindow<EditarProduto, EditarProdutoVM>();

            try
            {
                SessionProvider.MainSessionFactory = SessionProvider.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            var configJson = File.ReadAllText("Config.json");
            JsonConvert.PopulateObject(configJson, Config.Instancia);

            AbrirTelaProdutoComando = new RelayCommand(AbrirTelaProduto);
            AbrirTelaFornecedorComando = new RelayCommand(AbrirTelaFornecedor);
            AbrirTelaMarcaComando = new RelayCommand(AbrirTelaMarca);
            AbrirTelaLojaComando = new RelayCommand(AbrirTelaLoja);
            AbrirTelaRecebimentoComando = new RelayCommand(AbrirTelaRecebimento);
            AbrirTelaContagemComando = new RelayCommand(AbrirTelaContagem);
            AbrirTelaFolhaPagamentoComando = new RelayCommand(AbrirTelaFolhaPagamento);
            AbrirTelaFuncionarioComando = new RelayCommand(AbrirTelaFuncionario);
            AbrirTelaDespesasComando = new RelayCommand(AbrirTelaDespesas);
            AbrirTelaCompraFornecedorComando = new RelayCommand(AbrirTelaCompraFornecedor);
            AbrirTelaEntradaMercadoriaComando = new RelayCommand(AbrirTelaEntradaMercadoria);

            ResourceDictionary resourceDictionary = new ResourceDictionary();

            switch (CultureInfo.CurrentCulture.Name)
            {
                case "pt-BR":
                    resourceDictionary.Source = new Uri(@"..\Resources\Linguagem\PT-BR.xaml", UriKind.Relative);
                    break;
                case "en-US":
                    resourceDictionary.Source = new Uri(@"..\Resources\Linguagem\EN-US.xaml", UriKind.Relative);
                    break;
                default:
                    resourceDictionary.Source = new Uri(@"..\Resources\Linguagem\EN-US.xaml", UriKind.Relative);
                    break;
            }

            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        private void AbrirTelaEntradaMercadoria(object obj)
        {
            openView.Show(new PesquisarEntradaVM());
        }

        private void AbrirTelaCompraFornecedor(object obj)
        {
            openView.Show(new PesquisarCompraDeFornecedorVM());
        }

        private void AbrirTelaDespesas(object obj)
        {
            openView.Show(new PesquisarDespesaVM());
        }

        public void AbrirTelaProduto(object parameter)
        {
            openView.Show(new PesquisarProdutoVM());
        }
        public void AbrirTelaFornecedor(object parameter)
        {
            openView.Show(new PesquisarFornecedorVM());
        }
        public void AbrirTelaMarca(object parameter)
        {
            openView.Show(new PesquisarMarcaVM());
        }
        public void AbrirTelaLoja(object parameter)
        {
            openView.Show(new PesquisarLojaVM());
        }
        public void AbrirTelaRecebimento(object parameter)
        {
            openView.Show(new PesquisarRecebimentoVM());
        }
        public void AbrirTelaContagem(object parameter)
        {
            openView.Show(new PesquisarContagemVM());
        }
        public void AbrirTelaFolhaPagamento(object parameter)
        {
            openView.Show(new PesquisarFolhaVM());
        }
        public void AbrirTelaFuncionario(object p)
        {
            openView.Show(new PesquisarFuncionarioVM());
        }
    }
}

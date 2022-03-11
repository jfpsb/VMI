using Newtonsoft.Json;
using NHibernate;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel
{
    class VandaModaIntimaVM : ObservableObject
    {
        private IAbreTelaPesquisaService AbreTelaPesquisaService;
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

        private DAOEntradaMercadoriaProdutoGrade dao;

        public VandaModaIntimaVM(IAbreTelaPesquisaService abreTelaPesquisaService)
        {
            SessionProvider.MainSessionFactory = SessionProvider.BuildSessionFactory();

            AbreTelaPesquisaService = abreTelaPesquisaService;

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
            AbreTelaPesquisaService.AbrirTelaEntradaMercadoria();
        }

        //private async void CriarGradePadrao()
        //{
        //    ISession session = SessionProvider.GetSession();
        //    DAOProduto dao = new DAOProduto(session);
        //    DAOGrade daoGrade = new DAOGrade(session);

        //    var gradeDiversos = await daoGrade.ListarPorId(27);

        //    var produtos = await dao.Listar();

        //    if (produtos != null && produtos.Count > 0)
        //    {
        //        foreach (var produto in produtos)
        //        {
        //            if (produto.Grades.Count == 0)
        //            {
        //                Model.ProdutoGrade produtoGrade = new ProdutoGrade
        //                {
        //                    CodBarra = produto.CodBarra,
        //                    Produto = produto,
        //                    Preco = produto.Preco,
        //                    PrecoCusto = produto.PrecoCusto
        //                };

        //                Model.SubGrade subGrade = new SubGrade
        //                {
        //                    ProdutoGrade = produtoGrade,
        //                    Grade = gradeDiversos
        //                };

        //                produtoGrade.SubGrades.Add(subGrade);

        //                produto.Grades.Add(produtoGrade);

        //                var result = await dao.Atualizar(produto);

        //                Console.WriteLine(result);
        //            }
        //        }
        //    }

        //    SessionProvider.FechaSession(session);
        //}

        private void AbrirTelaCompraFornecedor(object obj)
        {
            AbreTelaPesquisaService.AbrirTelaCompraDeFornecedor();
        }

        private void AbrirTelaDespesas(object obj)
        {
            AbreTelaPesquisaService.AbrirTelaDespesas();
        }

        public void AbrirTelaProduto(object parameter)
        {
            AbreTelaPesquisaService.AbrirTelaProduto();
        }
        public void AbrirTelaFornecedor(object parameter)
        {
            AbreTelaPesquisaService.AbrirTelaFornecedor();
        }
        public void AbrirTelaMarca(object parameter)
        {
            AbreTelaPesquisaService.AbrirTelaMarca();
        }
        public void AbrirTelaLoja(object parameter)
        {
            AbreTelaPesquisaService.AbrirTelaLoja();
        }
        public void AbrirTelaRecebimento(object parameter)
        {
            AbreTelaPesquisaService.AbrirTelaRecebimento();
        }
        public void AbrirTelaContagem(object parameter)
        {
            AbreTelaPesquisaService.AbrirTelaContagem();
        }
        public void AbrirTelaFolhaPagamento(object parameter)
        {
            AbreTelaPesquisaService.AbrirTelaFolhaPagamento();
        }
        public void AbrirTelaFuncionario(object p)
        {
            AbreTelaPesquisaService.AbrirTelaFuncionario();
        }
    }
}

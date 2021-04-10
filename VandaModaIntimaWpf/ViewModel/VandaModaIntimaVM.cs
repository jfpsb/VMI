using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados;
using VandaModaIntimaWpf.Model;
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

        public VandaModaIntimaVM(IAbreTelaPesquisaService abreTelaPesquisaService)
        {
            AbreTelaPesquisaService = abreTelaPesquisaService;

            //Autenticando CouchDB
            CouchDbClient.Instancia.GetAuthenticationCookie();

            var configJson = File.ReadAllText("Config.json");
            JsonConvert.PopulateObject(configJson, Config.Instancia);

            //MqttClientInit mqttClientInit = new MqttClientInit();

            AbrirTelaProdutoComando = new RelayCommand(AbrirTelaProduto);
            AbrirTelaFornecedorComando = new RelayCommand(AbrirTelaFornecedor);
            AbrirTelaMarcaComando = new RelayCommand(AbrirTelaMarca);
            AbrirTelaLojaComando = new RelayCommand(AbrirTelaLoja);
            AbrirTelaRecebimentoComando = new RelayCommand(AbrirTelaRecebimento);
            AbrirTelaContagemComando = new RelayCommand(AbrirTelaContagem);
            AbrirTelaFolhaPagamentoComando = new RelayCommand(AbrirTelaFolhaPagamento);
            AbrirTelaFuncionarioComando = new RelayCommand(AbrirTelaFuncionario);
            AbrirTelaDespesasComando = new RelayCommand(AbrirTelaDespesas);

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

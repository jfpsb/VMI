using System;
using System.Globalization;
using System.ServiceModel.Channels;
using System.Text;
using System.Windows;
using System.Windows.Input;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.View.Contagem;
using VandaModaIntimaWpf.View.Duplicata;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.View.Fornecedor;
using VandaModaIntimaWpf.View.Funcionario;
using VandaModaIntimaWpf.View.Loja;
using VandaModaIntimaWpf.View.Marca;
using VandaModaIntimaWpf.View.Produto;
using VandaModaIntimaWpf.View.RecebimentoCartao;
using VMIMqttBroker;

namespace VandaModaIntimaWpf.ViewModel
{
    class VandaModaIntimaViewModel : ObservableObject
    {
        public ICommand AbrirTelaProdutoComando { get; set; }
        public ICommand AbrirTelaFornecedorComando { get; set; }
        public ICommand AbrirTelaMarcaComando { get; set; }
        public ICommand AbrirTelaLojaComando { get; set; }
        public ICommand AbrirTelaRecebimentoComando { get; set; }
        public ICommand AbrirTelaContagemComando { get; set; }
        public ICommand AbrirTelaFolhaPagamentoComando { get; set; }
        public ICommand AbrirTelaFuncionarioComando { get; set; }
        public ICommand AbrirTelaDespesasComando { get; set; }

        //TODO: Comando para tela de funcionário e despesas
        public VandaModaIntimaViewModel()
        {
            GlobalConfigs global = new GlobalConfigs();
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

            DuplicataHandler duplicataHandler = new DuplicataHandler();
            duplicataHandler.ShowDialog();
        }

        private void AbrirTelaDespesas(object obj)
        {

        }

        public void AbrirTelaProduto(object parameter)
        {
            PesquisarProduto pesquisarProduto = new PesquisarProduto();
            pesquisarProduto.Show();
        }
        public void AbrirTelaFornecedor(object parameter)
        {
            PesquisarFornecedor pesquisarFornecedor = new PesquisarFornecedor();
            pesquisarFornecedor.Show();
        }
        public void AbrirTelaMarca(object parameter)
        {
            PesquisarMarca pesquisarMarca = new PesquisarMarca();
            pesquisarMarca.Show();
        }
        public void AbrirTelaLoja(object parameter)
        {
            PesquisarLoja pesquisarLoja = new PesquisarLoja();
            pesquisarLoja.Show();
        }
        public void AbrirTelaRecebimento(object parameter)
        {
            PesquisarRecebimento pesquisarRecebimento = new PesquisarRecebimento();
            pesquisarRecebimento.Show();
        }
        public void AbrirTelaContagem(object parameter)
        {
            PesquisarContagem pesquisarContagem = new PesquisarContagem();
            pesquisarContagem.Show();
        }
        public void AbrirTelaFolhaPagamento(object parameter)
        {
            PesquisarFolhaPagamento pesquisarFolhaPagamento = new PesquisarFolhaPagamento();
            pesquisarFolhaPagamento.Show();
        }
        public void AbrirTelaFuncionario(object p)
        {
            PesquisarFuncionario pesquisarFuncionario = new PesquisarFuncionario();
            pesquisarFuncionario.Show();
        }
    }
}

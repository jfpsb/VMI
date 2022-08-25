using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.View.Avisos;
using VandaModaIntimaWpf.View.CompraDeFornecedor;
using VandaModaIntimaWpf.View.Contagem;
using VandaModaIntimaWpf.View.Despesa;
using VandaModaIntimaWpf.View.EntradaDeMercadoria;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.View.FolhaPagamento.CalculoDeBonusMensalPorDia;
using VandaModaIntimaWpf.View.Fornecedor;
using VandaModaIntimaWpf.View.Funcionario;
using VandaModaIntimaWpf.View.Grade;
using VandaModaIntimaWpf.View.Loja;
using VandaModaIntimaWpf.View.Marca;
using VandaModaIntimaWpf.View.PontoEletronico;
using VandaModaIntimaWpf.View.Produto;
using VandaModaIntimaWpf.View.RecebimentoCartao;
using VandaModaIntimaWpf.View.Representante;
using VandaModaIntimaWpf.View.TipoGrade;
using VandaModaIntimaWpf.ViewModel.Avisos;
using VandaModaIntimaWpf.ViewModel.CompraDeFornecedor;
using VandaModaIntimaWpf.ViewModel.Contagem;
using VandaModaIntimaWpf.ViewModel.Despesa;
using VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria;
using VandaModaIntimaWpf.ViewModel.FolhaPagamento;
using VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia;
using VandaModaIntimaWpf.ViewModel.Fornecedor;
using VandaModaIntimaWpf.ViewModel.Funcionario;
using VandaModaIntimaWpf.ViewModel.Grade;
using VandaModaIntimaWpf.ViewModel.Loja;
using VandaModaIntimaWpf.ViewModel.Marca;
using VandaModaIntimaWpf.ViewModel.PontoEletronico;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;

namespace VandaModaIntimaWpf.ViewModel
{
    class VandaModaIntimaVM : ObservableObject
    {
        public VandaModaIntimaVM()
        {
            WindowService.RegistrarWindow<RegistrarPonto, RegistrarPontoVM>();

            WindowService.RegistrarWindow<TelaDeLoginSimples, InserirSenhaPontoVM>();

            WindowService.RegistrarWindow<SalvarEntradaDeMercadoria, EditarEntradaDeMercadoriaVM>();

            WindowService.RegistrarWindow<TrocarSenhaFuncionario, TrocarSenhaFuncionarioVM>();

            WindowService.RegistrarWindow<PesquisarRepresentante, PesquisarRepresentanteVM>();

            WindowService.RegistrarWindow<ConsolidarPontosEletronicos, ConsolidarPontosEletronicosVM>();

            WindowService.RegistrarWindow<ConfirmarConsolidacaoPontosEletronicos, ConfirmarConsolidacaoPontosEletronicosVM>();

            WindowService.RegistrarWindow<ApresentaQRCodePix, ApresentaQRCodePixVM>();

            WindowService.RegistrarWindow<ConfigurarCredenciaisPix, ConfiguraCredenciaisPixVM>();

            WindowService.RegistrarWindow<MaisDetalhesPix, MaisDetalhesPixVM>();

            try
            {
                SessionProvider.MainSessionFactory = SessionProvider.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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
    }
}

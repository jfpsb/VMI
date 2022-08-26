using System;
using System.Globalization;
using System.Windows;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.View.EntradaDeMercadoria;
using VandaModaIntimaWpf.View.Pix;
using VandaModaIntimaWpf.View.PontoEletronico;
using VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria;
using VandaModaIntimaWpf.ViewModel.Pix;
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

using NHibernate;
using System.Windows;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.View.Avisos;
using VandaModaIntimaWpf.View.CompraDeFornecedor;
using VandaModaIntimaWpf.View.Contagem;
using VandaModaIntimaWpf.View.Despesa;
using VandaModaIntimaWpf.View.EntradaDeMercadoria;
using VandaModaIntimaWpf.View.Ferias;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.View.Fornecedor;
using VandaModaIntimaWpf.View.Funcionario;
using VandaModaIntimaWpf.View.Loja;
using VandaModaIntimaWpf.View.Marca;
using VandaModaIntimaWpf.View.PontoEletronico;
using VandaModaIntimaWpf.ViewModel.PontoEletronico;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;

namespace VandaModaIntimaWpf.View
{
    public partial class VandaModaIntima : Window
    {
        public VandaModaIntima()
        {
            InitializeComponent();
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        private void TelaPrincipal_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SessionProvider.FechaSessionFactory();
        }

        private void TelaPrincipal_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BtnPontoEletronico_Click(object sender, RoutedEventArgs e)
        {
            ISession session = SessionProvider.GetSession();
            new WindowService().ShowDialog(new RegistrarPontoVM(session), (result, vm) =>
            {
                SessionProvider.FechaSession(session);
            });
        }

        private void BtnPix_Click(object sender, RoutedEventArgs e)
        {
            PesquisarPix pesquisarPix = new PesquisarPix();
            pesquisarPix.Show();
        }
    }
}

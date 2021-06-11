using NHibernate;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class CadastrarFornecedorOnlineVM : CadastrarFornecedorManualmenteVM
    {
        public ICommand PesquisarComando { get; set; }

        public CadastrarFornecedorOnlineVM(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            _session = session;
            VisibilidadeBotaoPesquisar = Visibility.Visible;
            PesquisarComando = new RelayCommand(PesquisarFornecedor, (object p) => { return Entidade.Cnpj?.Length == 14; });
        }

        public CadastrarFornecedorOnlineVM(ISession session, IMessageBoxService messageBoxService, string cnpj, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            _session = session;
            VisibilidadeBotaoPesquisar = Visibility.Visible;
            PesquisarComando = new RelayCommand(PesquisarFornecedor, (object p) => { return Entidade.Cnpj?.Length == 14; });

            Entidade.Cnpj = cnpj;
            PesquisarFornecedor(null);
        }

        public async void PesquisarFornecedor(object parameter)
        {
            try
            {
                Entidade = await new RequisicaoReceitaFederal().GetFornecedor(Entidade.Cnpj);
                MessageBoxService.Show("Fornecedor Encontrado");
            }
            catch (WebException we)
            {
                if (we.Message.Contains("429"))
                {
                    MessageBoxService.Show("Aguarde Um Pouco Entre Cada Consulta");
                }
                else
                {
                    MessageBoxService.Show(we.Message);
                }
            }
            catch (InvalidDataException ide)
            {
                MessageBoxService.Show(ide.Message);
            }
        }
    }
}

using NHibernate;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class CadastrarFornecedorOnlineVM : CadastrarFornecedorManualmenteVM
    {
        public ICommand PesquisarComando { get; set; }

        public CadastrarFornecedorOnlineVM(ISession session) : base(session, false)
        {
            VisibilidadeBotaoPesquisar = Visibility.Visible;
            PesquisarComando = new RelayCommand(PesquisarFornecedor, (object p) => { return Entidade.Cnpj?.Length == 14; });
        }

        public CadastrarFornecedorOnlineVM(ISession session, string cnpj) : base(session, false)
        {
            //TODO: parametros, cnpj
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

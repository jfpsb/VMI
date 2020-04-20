using NHibernate;
using System.IO;
using System.Net;
using System.Windows.Input;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class CadastrarFornecedorOnlineViewModel : CadastrarFornecedorManualmenteViewModel
    {
        public ICommand PesquisarComando { get; set; }

        public CadastrarFornecedorOnlineViewModel(ISession session) : base(session)
        {
            _session = session;
            PesquisarComando = new RelayCommand(PesquisarFornecedor, (object p) => { return Fornecedor.Cnpj?.Length == 14; });
        }

        public async void PesquisarFornecedor(object parameter)
        {
            try
            {
                Fornecedor = await new RequisicaoReceitaFederal().GetFornecedor(Fornecedor.Cnpj);
                SetStatusBarSucessoPesquisa();
            }
            catch (WebException we)
            {
                if (we.Message.Contains("429"))
                {
                    SetStatusBarErroPesquisa("Aguarde Um Pouco Entre Cada Consulta");
                }
                else
                {
                    SetStatusBarErroPesquisa(we.Message);
                }
            }
            catch (InvalidDataException ide)
            {
                SetStatusBarErroPesquisa(ide.Message);
            }
        }

        private void SetStatusBarErroPesquisa(string mensagem)
        {
            MensagemStatusBar = mensagem;
            ImagemStatusBar = IMAGEMERRO;
        }

        private void SetStatusBarSucessoPesquisa()
        {
            MensagemStatusBar = "Fornecedor Encontrado";
            ImagemStatusBar = IMAGEMSUCESSO;
        }
    }
}

using NHibernate;
using System.IO;
using System.Net;
using System.Windows.Input;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class CadastrarFornOnlineVM : CadastrarFornecedorManualmenteVM
    {
        public ICommand PesquisarComando { get; set; }

        public CadastrarFornOnlineVM(ISession session, IMessageBoxService messageBoxService) : base(session, messageBoxService)
        {
            _session = session;
            PesquisarComando = new RelayCommand(PesquisarFornecedor, (object p) => { return Entidade.Cnpj?.Length == 14; });
        }

        public async void PesquisarFornecedor(object parameter)
        {
            try
            {
                Entidade = await new RequisicaoReceitaFederal().GetFornecedor(Entidade.Cnpj);
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

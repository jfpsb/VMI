using Newtonsoft.Json;
using NHibernate;
using System.Threading.Tasks;
using VandaModaIntimaWpf.BancoDeDados;
using VandaModaIntimaWpf.BancoDeDados.Model;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    public class EditarFuncVM : CadastrarFuncVM
    {
        public EditarFuncVM(ISession session, IMessageBoxService messageBoxService) : base(session, messageBoxService) { }

        public async override void InserirNoBancoDeDados(AposCriarDocumentoEventArgs e)
        {
            await AtualizarNoBancoDeDados(e);
        }
        protected async override Task<AposCriarDocumentoEventArgs> ExecutarSalvar()
        {
            CouchDbResponse couchDbResponse;
            AposCriarDocumentoEventArgs e = new AposCriarDocumentoEventArgs();

            if (ultimoLog != null)
            {
                e.CouchDbLog = (CouchDbFuncionarioLog)ultimoLog.Clone();
                ultimoLog.AtribuiCampos(Entidade);
                couchDbResponse = await couchDbClient.UpdateDocument(ultimoLog);
                e.CouchDbLog.Rev = couchDbResponse.Rev;
            }
            else
            {
                string jsonData = JsonConvert.SerializeObject(Entidade);
                couchDbResponse = await couchDbClient.CreateDocument(Entidade.Cpf, jsonData);
            }

            e.CouchDbResponse = couchDbResponse;
            e.MensagemSucesso = cadastrarViewModelStrategy.MensagemDocumentoAtualizadoSucesso();
            e.MensagemErro = cadastrarViewModelStrategy.MensagemDocumentoNaoAtualizado();
            e.ObjetoSalvo = Entidade;

            return e;
        }
    }
}

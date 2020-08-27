using Newtonsoft.Json;
using NHibernate;
using System.Threading.Tasks;
using VandaModaIntimaWpf.BancoDeDados;
using VandaModaIntimaWpf.BancoDeDados.Model;
using VandaModaIntimaWpf.Resources;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class EditarLojaViewModel : CadastrarLojaViewModel
    {
        public EditarLojaViewModel(ISession session) : base(session)
        {
            if (Entidade.Matriz == null)
                Entidade.Matriz = Matrizes[0];
        }
        protected async override Task<AposCriarDocumentoEventArgs> ExecutarSalvar()
        {
            CouchDbResponse couchDbResponse;
            AposCriarDocumentoEventArgs e = new AposCriarDocumentoEventArgs();

            if (ultimoLog != null)
            {
                e.CouchDbLog = (CouchDbLojaLog)ultimoLog.Clone();
                ultimoLog.AtribuiCampos(Entidade);
                couchDbResponse = await couchDbClient.UpdateDocument(ultimoLog);
                e.CouchDbLog.Rev = couchDbResponse.Rev;
            }
            else
            {
                string jsonData = JsonConvert.SerializeObject(Entidade);
                couchDbResponse = await couchDbClient.CreateDocument(Entidade.Cnpj, jsonData);
            }

            e.CouchDbResponse = couchDbResponse;
            e.MensagemSucesso = cadastrarViewModelStrategy.MensagemDocumentoAtualizadoSucesso();
            e.MensagemErro = cadastrarViewModelStrategy.MensagemDocumentoNaoAtualizado();
            e.ObjetoSalvo = Entidade;

            return e;
        }
        public async override void InserirNoBancoDeDados(AposCriarDocumentoEventArgs e)
        {
            await AtualizarNoBancoDeDados(e);
        }
        public LojaModel MatrizComboBox
        {
            get
            {
                if (Entidade.Matriz == null)
                {
                    Entidade.Matriz = new LojaModel(GetResource.GetString("matriz_nao_selecionada"));
                }

                return Entidade.Matriz;
            }

            set
            {
                Entidade.Matriz = value;
                OnPropertyChanged("MatrizComboBox");
            }
        }
    }
}

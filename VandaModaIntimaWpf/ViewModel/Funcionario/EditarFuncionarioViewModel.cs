using Newtonsoft.Json;
using NHibernate;
using VandaModaIntimaWpf.BancoDeDados;
using VandaModaIntimaWpf.BancoDeDados.Model;
using VandaModaIntimaWpf.Model.DAO.MySQL;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    public class EditarFuncionarioViewModel : CadastrarFuncionarioViewModel
    {
        public EditarFuncionarioViewModel(ISession session) : base(session) { }

        public override async void Salvar(object parameter)
        {
            if (Funcionario.Loja.Cnpj == null)
                Funcionario.Loja = null;

            CouchDbResponse couchDbResponse;
            AposCriarDocumentoEventArgs e = new AposCriarDocumentoEventArgs();

            if (ultimoLog != null)
            {
                e.CouchDbLog = (CouchDbFuncionarioLog)ultimoLog.Clone();
                ultimoLog.AtribuiCampos(Funcionario);
                couchDbResponse = await couchDbClient.UpdateDocument(ultimoLog);
                e.CouchDbLog.Rev = couchDbResponse.Rev;
            }
            else
            {
                string jsonData = JsonConvert.SerializeObject(Funcionario);
                couchDbResponse = await couchDbClient.CreateDocument(Funcionario.Cpf, jsonData);
            }

            e.CouchDbResponse = couchDbResponse;
            e.MensagemSucesso = "LOG de Atualização de Funcionario Criado com Sucesso";
            e.MensagemErro = "Erro ao Criar Log de Atualização de Funcionario";
            e.ObjetoSalvo = Funcionario;

            ChamaAposCriarDocumento(e);
        }

        public async override void InserirNoBancoDeDados(AposCriarDocumentoEventArgs e)
        {
            if (e.CouchDbResponse.Ok)
            {
                _result = await daoFuncionario.Merge(Funcionario);

                AposInserirBDEventArgs e2 = new AposInserirBDEventArgs()
                {
                    InseridoComSucesso = _result,
                    IssoEUmUpdate = true,
                    MensagemSucesso = "Funcionario Atualizado com Sucesso",
                    MensagemErro = "Erro ao Atualizar Funcionario",
                    ObjetoSalvo = Funcionario,
                    CouchDbLog = e.CouchDbLog
                };

                ChamaAposInserirNoBD(e2);
            }
        }
    }
}

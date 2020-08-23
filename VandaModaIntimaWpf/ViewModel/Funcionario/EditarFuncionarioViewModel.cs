using Newtonsoft.Json;
using NHibernate;
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

            string funcionarioJson = JsonConvert.SerializeObject(Funcionario);
            var couchDbResponse = await couchDbClient.CreateOrUpdateDocument(Funcionario.Cpf, funcionarioJson);

            AposCriarDocumentoEventArgs e = new AposCriarDocumentoEventArgs()
            {
                CouchDbResponse = couchDbResponse,
                MensagemSucesso = "LOG de Atualização de Funcionário Criado com Sucesso",
                MensagemErro = "Erro ao Criar Log de Atualização de Funcionário",
                ObjetoSalvo = Funcionario
            };

            ChamaAposCriarDocumento(e);
        }

        public async override void InserirNoBancoDeDados(AposCriarDocumentoEventArgs e)
        {
            if (Funcionario.Loja.Cnpj == null)
                Funcionario.Loja = null;

            if (e.CouchDbResponse.Ok)
            {
                _result = await daoFuncionario.Merge(Funcionario);

                AposInserirBDEventArgs e2 = new AposInserirBDEventArgs()
                {
                    InseridoComSucesso = _result,
                    MensagemSucesso = "Funcionário Atualizado com Sucesso",
                    MensagemErro = "Erro ao Atualizar Funcionário",
                    ObjetoSalvo = Funcionario
                };

                ChamaAposInserirNoBD(e2);
            }
            else
            {
                //TODO: Reverter update
                //CouchDbResponse couchDbResponse = await couchDbClient.CreateOrUpdateDocument(Produto.CodBarra, );
                //Console.WriteLine(string.Format("DELETANDO {0}: {1}", couchDbResponse.Id, couchDbResponse.Ok));
            }
        }
    }
}

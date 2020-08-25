using Newtonsoft.Json;
using NHibernate;
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
            if (Loja.Matriz == null)
                Loja.Matriz = Matrizes[0];
        }

        //TODO: strings em resources
        public override async void Salvar(object parameter)
        {
            if (Loja.Matriz.Cnpj == null)
                Loja.Matriz = null;

            CouchDbResponse couchDbResponse;
            AposCriarDocumentoEventArgs e = new AposCriarDocumentoEventArgs();

            if (ultimoLog != null)
            {
                e.CouchDbLog = (CouchDbLojaLog)ultimoLog.Clone();
                ultimoLog.AtribuiCampos(Loja);
                couchDbResponse = await couchDbClient.UpdateDocument(ultimoLog);
                e.CouchDbLog.Rev = couchDbResponse.Rev;
            }
            else
            {
                string jsonData = JsonConvert.SerializeObject(Loja);
                couchDbResponse = await couchDbClient.CreateDocument(Loja.Cnpj, jsonData);
            }

            e.CouchDbResponse = couchDbResponse;
            e.MensagemSucesso = "LOG de Atualização de Loja Criado com Sucesso";
            e.MensagemErro = "Erro ao Criar Log de Atualização de Loja";
            e.ObjetoSalvo = Loja;

            ChamaAposCriarDocumento(e);
        }

        public async override void InserirNoBancoDeDados(AposCriarDocumentoEventArgs e)
        {
            if (e.CouchDbResponse.Ok)
            {
                _result = await daoLoja.Merge(Loja);

                AposInserirBDEventArgs e2 = new AposInserirBDEventArgs()
                {
                    InseridoComSucesso = _result,
                    IssoEUmUpdate = true,
                    MensagemSucesso = "Loja Atualizada com Sucesso",
                    MensagemErro = "Erro ao Atualizar Loja",
                    ObjetoSalvo = Loja,
                    CouchDbLog = e.CouchDbLog
                };

                ChamaAposInserirNoBD(e2);
            }
        }

        public new LojaModel Loja
        {
            get { return lojaModel; }
            set
            {
                lojaModel = value;
                OnPropertyChanged("Loja");
                OnPropertyChanged("MatrizComboBox");
            }
        }

        public LojaModel MatrizComboBox
        {
            get
            {
                if (Loja.Matriz == null)
                {
                    Loja.Matriz = new LojaModel(GetResource.GetString("matriz_nao_selecionada"));
                }

                return Loja.Matriz;
            }

            set
            {
                Loja.Matriz = value;
                OnPropertyChanged("MatrizComboBox");
            }
        }
    }
}

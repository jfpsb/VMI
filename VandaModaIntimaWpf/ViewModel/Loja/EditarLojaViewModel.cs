using Newtonsoft.Json;
using NHibernate;
using System.Windows;
using VandaModaIntimaWpf.Resources;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class EditarLojaViewModel : CadastrarLojaViewModel
    {
        public EditarLojaViewModel(ISession session) : base(session)
        {
            //if (Loja.Matriz == null)
            //    Loja.Matriz = Matrizes[0];
        }

        //TODO: strings em resources
        public override async void Salvar(object parameter)
        {
            if (Loja.Matriz.Cnpj == null)
                Loja.Matriz = null;

            string lojaJson = JsonConvert.SerializeObject(Loja);
            var couchDbResponse = await couchDbClient.CreateOrUpdateDocument(Loja.Cnpj, lojaJson);

            AposCriarDocumentoEventArgs e = new AposCriarDocumentoEventArgs()
            {
                CouchDbResponse = couchDbResponse,
                MensagemSucesso = "LOG de Atualização de Loja Criado com Sucesso",
                MensagemErro = "Erro ao Criar Log de Atualização de Loja",
                ObjetoSalvo = Loja
            };

            ChamaAposCriarDocumento(e);
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

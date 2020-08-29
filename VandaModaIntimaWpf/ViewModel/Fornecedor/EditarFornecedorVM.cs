using Newtonsoft.Json;
using NHibernate;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.BancoDeDados.Model;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class EditarFornecedorVM : CadastrarFornManualmenteVM
    {
        public ICommand AtualizarReceitaComando { get; set; }
        public EditarFornecedorVM(ISession session, IMessageBoxService messageBoxService) : base(session, messageBoxService)
        {
            AtualizarReceitaComando = new RelayCommand(AtualizarReceita);
        }
        protected async override Task<AposCriarDocumentoEventArgs> ExecutarSalvar()
        {
            CouchDbResponse couchDbResponse;
            AposCriarDocumentoEventArgs e = new AposCriarDocumentoEventArgs();

            if (ultimoLog != null)
            {
                e.CouchDbLog = (CouchDbFornecedorLog)ultimoLog.Clone();
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
            e.MensagemSucesso = "LOG de Atualização de Fornecedor Criado com Sucesso";
            e.MensagemErro = "Erro ao Criar Log de Atualização de Fornecedor";
            e.ObjetoSalvo = Entidade;

            return e;
        }
        public async override void InserirNoBancoDeDados(AposCriarDocumentoEventArgs e)
        {
            await AtualizarNoBancoDeDados(e);
        }
        private async void AtualizarReceita(object parameter)
        {
            SetStatusBarAguardando("Pesquisando CNPJ na Receita Federal. Aguarde.");

            try
            {
                FornecedorModel result = await new RequisicaoReceitaFederal().GetFornecedor(Entidade.Cnpj);

                Entidade.Nome = result.Nome;
                Entidade.Fantasia = result.Fantasia;
                Entidade.Email = result.Email;
                Entidade.Telefone = result.Telefone;
                // Chama OnPropertyChanged para atualizar na View os valores atribuídos a Fornecedor
                OnPropertyChanged("Fornecedor");

                SetStatusBarSucesso("Pesquisa Realizada Com Sucesso.");
            }
            catch (WebException we)
            {
                if (we.Message.Contains("429"))
                {
                    SetStatusBarErro("Muitas Pesquisas Realizadas Sucessivamente. Aguarde Um Pouco.");
                }
                else
                {
                    SetStatusBarErro(we.Message);
                }
            }
            catch (InvalidDataException ide)
            {
                SetStatusBarErro(ide.Message);
            }
        }
    }
}

using Newtonsoft.Json;
using NHibernate;
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.BancoDeDados.Model;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class EditarFornecedorViewModel : CadastrarFornecedorManualmenteViewModel
    {
        public ICommand AtualizarReceitaComando { get; set; }
        public EditarFornecedorViewModel(ISession session) : base(session)
        {
            AtualizarReceitaComando = new RelayCommand(AtualizarReceita);
        }
        public override async void Salvar(object parameter)
        {
            CouchDbResponse couchDbResponse;
            AposCriarDocumentoEventArgs e = new AposCriarDocumentoEventArgs();

            if (ultimoLog != null)
            {
                e.CouchDbLog = (CouchDbFornecedorLog)ultimoLog.Clone();
                ultimoLog.AtribuiCampos(Fornecedor);
                couchDbResponse = await couchDbClient.UpdateDocument(ultimoLog);
                e.CouchDbLog.Rev = couchDbResponse.Rev;
            }
            else
            {
                string jsonData = JsonConvert.SerializeObject(Fornecedor);
                couchDbResponse = await couchDbClient.CreateDocument(Fornecedor.Cnpj, jsonData);
            }

            e.CouchDbResponse = couchDbResponse;
            e.MensagemSucesso = "LOG de Atualização de Fornecedor Criado com Sucesso";
            e.MensagemErro = "Erro ao Criar Log de Atualização de Fornecedor";
            e.ObjetoSalvo = Fornecedor;

            ChamaAposCriarDocumento(e);
        }

        public async override void InserirNoBancoDeDados(AposCriarDocumentoEventArgs e)
        {
            if (e.CouchDbResponse.Ok)
            {
                _result = await daoFornecedor.Merge(Fornecedor);

                AposInserirBDEventArgs e2 = new AposInserirBDEventArgs()
                {
                    InseridoComSucesso = _result,
                    IssoEUmUpdate = true,
                    MensagemSucesso = "Fornecedor Atualizado com Sucesso",
                    MensagemErro = "Erro ao Atualizar Fornecedor",
                    ObjetoSalvo = Fornecedor,
                    CouchDbLog = e.CouchDbLog
                };

                ChamaAposInserirNoBD(e2);
            }
        }

        private async void AtualizarReceita(object parameter)
        {
            SetStatusBarAguardando("Pesquisando CNPJ na Receita Federal. Aguarde.");

            try
            {
                FornecedorModel result = await new RequisicaoReceitaFederal().GetFornecedor(Fornecedor.Cnpj);

                Fornecedor.Nome = result.Nome;
                Fornecedor.Fantasia = result.Fantasia;
                Fornecedor.Email = result.Email;
                Fornecedor.Telefone = result.Telefone;
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

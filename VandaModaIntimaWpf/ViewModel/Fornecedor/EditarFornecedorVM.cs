using NHibernate;
using System.IO;
using System.Net;
using System.Windows.Input;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class EditarFornecedorVM : CadastrarFornecedorManualmenteVM
    {
        public ICommand AtualizarReceitaComando { get; set; }
        public EditarFornecedorVM(ISession session, FornecedorModel fornecedor, IMessageBoxService messageBoxService) : base(session, messageBoxService)
        {
            viewModelStrategy = new EditarFornecedorVMStrategy();
            Entidade = fornecedor;
            AtualizarReceitaComando = new RelayCommand(AtualizarReceita);
        }
        private async void AtualizarReceita(object parameter)
        {
            SetStatusBarAguardando("Pesquisando CNPJ na Receita Federal. Aguarde.");

            try
            {
                FornecedorModel result = await new RequisicaoReceitaFederal().GetFornecedor(Entidade.Cnpj);
                await daoEntidade.Merge(result);

                //Entidade.Nome = result.Nome;
                //Entidade.Fantasia = result.Fantasia;
                //Entidade.Email = result.Email;
                //Entidade.Telefone = result.Telefone;
                //// Chama OnPropertyChanged para atualizar na View os valores atribuídos a Fornecedor
                OnPropertyChanged("Entidade");

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

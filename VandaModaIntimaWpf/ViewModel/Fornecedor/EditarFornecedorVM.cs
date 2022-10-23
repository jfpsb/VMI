using NHibernate;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class EditarFornecedorVM : CadastrarFornecedorManualmenteVM
    {
        public ICommand AtualizarReceitaComando { get; set; }
        public EditarFornecedorVM(ISession session, Model.Fornecedor fornecedor) : base(session, true)
        {
            Entidade = fornecedor;
            VisibilidadeBotaoAtualizarReceita = Visibility.Visible;
            viewModelStrategy = new EditarFornecedorVMStrategy();
            AtualizarReceitaComando = new RelayCommand(AtualizarReceita);
        }
        private async void AtualizarReceita(object parameter)
        {
            _messageBoxService.Show("Pesquisando CNPJ na Receita Federal. Aguarde.", viewModelStrategy.MessageBoxCaption());

            try
            {
                Model.Fornecedor result = await new RequisicaoReceitaFederal().GetFornecedor(Entidade.Cnpj);
                Entidade.CopiaDadosDeConsultaNaReceita(result);
                await daoEntidade.Atualizar(Entidade);
                OnPropertyChanged("Entidade");
                _messageBoxService.Show("Pesquisa Realizada Com Sucesso.", viewModelStrategy.MessageBoxCaption());
            }
            catch (WebException we)
            {
                if (we.Message.Contains("429"))
                {
                    _messageBoxService.Show("Muitas Pesquisas Realizadas Sucessivamente. Aguarde Um Pouco.", viewModelStrategy.MessageBoxCaption());
                }
                else
                {
                    _messageBoxService.Show(we.Message);
                }
            }
            catch (InvalidDataException ide)
            {
                _messageBoxService.Show(ide.Message);
            }
        }
    }
}

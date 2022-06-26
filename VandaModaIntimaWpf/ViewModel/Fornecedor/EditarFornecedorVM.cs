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
            MessageBoxService.Show("Pesquisando CNPJ na Receita Federal. Aguarde.", viewModelStrategy.MessageBoxCaption());

            try
            {
                var representante = Entidade.Representante;
                Model.Fornecedor result = await new RequisicaoReceitaFederal().GetFornecedor(Entidade.Cnpj);
                result.Representante = representante;
                await daoEntidade.Merge(result);
                OnPropertyChanged("Entidade");
                MessageBoxService.Show("Pesquisa Realizada Com Sucesso.", viewModelStrategy.MessageBoxCaption());
            }
            catch (WebException we)
            {
                if (we.Message.Contains("429"))
                {
                    MessageBoxService.Show("Muitas Pesquisas Realizadas Sucessivamente. Aguarde Um Pouco.", viewModelStrategy.MessageBoxCaption());
                }
                else
                {
                    MessageBoxService.Show(we.Message);
                }
            }
            catch (InvalidDataException ide)
            {
                MessageBoxService.Show(ide.Message);
            }
        }
    }
}

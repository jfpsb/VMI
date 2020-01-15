using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.View;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class EditarFornecedorViewModel : CadastrarFornecedorManualmenteViewModel, IEditarViewModel
    {
        private bool IsEditted = false;
        public ICommand AtualizarReceitaComando { get; set; }
        public EditarFornecedorViewModel() : base()
        {
            AtualizarReceitaComando = new RelayCommand(AtualizarReceita);
        }
        public override async void Salvar(object parameter)
        {
            var result = IsEditted = await daoFornecedor.Atualizar(Fornecedor);

            if (result)
            {
                await SetStatusBarSucesso("Atualização Realizada Com Sucesso");
            }
            else
            {
                SetStatusBarErro("Erro ao Atualizar Fornecedor");
            }
        }
        public bool EdicaoComSucesso()
        {
            return IsEditted;
        }
        public async void PassaId(object Id)
        {
            Fornecedor = await SessionProvider.GetSession().LoadAsync<FornecedorModel>(Id);
        }
        private async void AtualizarReceita(object parameter)
        {
            try
            {
                FornecedorModel result = await new RequisicaoReceitaFederal().GetFornecedor(Fornecedor.Cnpj);

                Fornecedor.Nome = result.Nome;
                Fornecedor.Fantasia = result.Fantasia;
                Fornecedor.Email = result.Email;
                // Chama OnPropertyChanged para atualizar na View os valores atribuídos a Fornecedor
                OnPropertyChanged("Fornecedor");
            }
            catch (WebException we)
            {
                Console.WriteLine(we.Message);
            }
            catch (InvalidDataException ide)
            {

            }
        }
    }
}

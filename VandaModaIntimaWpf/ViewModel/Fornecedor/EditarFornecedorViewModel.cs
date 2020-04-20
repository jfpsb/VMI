using NHibernate;
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
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
            var result = await daoFornecedor.Merge(Fornecedor);

            if (result)
            {
                await SetStatusBarSucesso($"Fornecedor {Fornecedor.Cnpj} Atualizado Com Sucesso");
            }
            else
            {
                SetStatusBarErro("Erro ao Atualizar Fornecedor");
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

                await SetStatusBarSucesso("Pesquisa Realizada Com Sucesso.");
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

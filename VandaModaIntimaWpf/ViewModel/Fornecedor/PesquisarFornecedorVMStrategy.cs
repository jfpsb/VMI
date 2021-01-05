using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.View.Fornecedor;
using VandaModaIntimaWpf.View.SQL;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.SQL;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class PesquisarFornecedorVMStrategy : IPesquisarMsgVMStrategy<FornecedorModel>
    {
        public string MensagemApagarEntidadeCerteza(FornecedorModel e)
        {
            return string.Format(GetResource.GetString("certeza_deletar_fornecedor"), e.Nome);
        }

        public string MensagemApagarMarcados()
        {
            return GetResource.GetString("deseja_apagar_fornecedor_marcados");
        }

        public string MensagemEntidadeDeletada(FornecedorModel e)
        {
            return string.Format(GetResource.GetString("fornecedor_deletado_com_sucesso"), e.Nome);
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return GetResource.GetString("fornecedor_nao_deletado");
        }

        public string MensagemEntidadesDeletadas()
        {
            return GetResource.GetString("fornecedores_deletados_com_sucesso");
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return GetResource.GetString("fornecedores_nao_deletados");
        }

        public string TelaApagarCaption()
        {
            return GetResource.GetString("apagar_fornecedores");
        }

        public void AbrirCadastrarOnline(ISession session)
        {
            CadastrarFornOnlineVM viewModel = new CadastrarFornOnlineVM(session, new MessageBoxService(), false);
            CadastrarFornecedorOnline cadastrarFornecedorOnline = new CadastrarFornecedorOnline
            {
                DataContext = viewModel
            };
            cadastrarFornecedorOnline.ShowDialog();
        }

        public string MensagemDocumentoDeletado()
        {
            return "LOG de Fornecedor Marcado Como Deletado Com Sucesso";
        }

        public string MensagemDocumentoNaoDeletado()
        {
            return "Erro ao Marcar LOG de Fornecedor Como Deletado";
        }
    }
}

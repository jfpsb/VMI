using System;

namespace VandaModaIntimaWpf.ViewModel.CompraDeFornecedor
{
    public class PesquisarCompraDeFornecedorVMStrategy : IPesquisarMsgVMStrategy<Model.CompraDeFornecedor>
    {
        public string MensagemApagarEntidadeCerteza(Model.CompraDeFornecedor e)
        {
            return "Tem Certeza Que Deseja Deletar Esta Compra de Fornecedor?";
        }

        public string MensagemApagarMarcados()
        {
            return "Tem Certeza Que Deseja Apagar As Compras De Fornecedor Marcadas?";
        }

        public string MensagemEntidadeDeletada(Model.CompraDeFornecedor e)
        {
            return "Compra De Fornecedor Foi Deletada Com Sucesso!";
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return "Erro Ao Deletar Compra De Fornecedor!";
        }

        public string MensagemEntidadesDeletadas()
        {
            return "Sucesso Ao Deletar Compras de Fornecedor Marcadas!";
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return "Erro Ao Deletar Compras De Fornecedor Marcadas!";
        }

        public string PesquisarEntidadeCaption()
        {
            return "Pesquisa De Compras De Fornecedor";
        }

        public string TelaApagarCaption()
        {
            return "Compras De Fornecedor";
        }
    }
}

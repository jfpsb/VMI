using VandaModaIntimaWpf.Resources;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    class PesqProdutoMsgVMStrategy : IPesquisarMsgVMStrategy<ProdutoModel>
    {
        public string MensagemApagarEntidadeCerteza(ProdutoModel e)
        {
            return string.Format(GetResource.GetString("certeza_deletar_produto"), e.Descricao);
        }
        public string MensagemApagarMarcados()
        {
            return GetResource.GetString("deseja_apagar_produto_marcados");
        }
        public string MensagemEntidadeDeletada(ProdutoModel e)
        {
            return string.Format(GetResource.GetString("produto_deletado_com_sucesso"), e.Descricao);
        }
        public string MensagemEntidadeNaoDeletada()
        {
            return GetResource.GetString("produto_nao_deletado");
        }
        public string MensagemEntidadesDeletadas()
        {
            return GetResource.GetString("produtos_deletados_com_sucesso");
        }
        public string MensagemEntidadesNaoDeletadas()
        {
            return GetResource.GetString("produtos_nao_deletados");
        }
        public string TelaApagarCaption()
        {
            return GetResource.GetString("apagar_produtos");
        }
        public string MensagemDocumentoDeletado()
        {
            return "LOG de Produto Marcado Como Deletado Com Sucesso";
        }
        public string MensagemDocumentoNaoDeletado()
        {
            return "Erro ao Marcar LOG de Produto Como Deletado";
        }

        public string PesquisarEntidadeCaption()
        {
            return "Pesquisa de Produto";
        }
    }
}

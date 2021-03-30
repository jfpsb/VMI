using VandaModaIntimaWpf.Resources;
using RecebimentoCartaoModel = VandaModaIntimaWpf.Model.RecebimentoCartao;

namespace VandaModaIntimaWpf.ViewModel.RecebimentoCartao
{
    class PesquisarRecebMsgVMStrategy : IPesquisarMsgVMStrategy<RecebimentoCartaoModel>
    {
        public string MensagemApagarEntidadeCerteza(RecebimentoCartaoModel e)
        {
            return string.Format(GetResource.GetString("certeza_deletar_recebimento"), e.MesAno, e.Loja.Nome);
        }

        public string MensagemApagarMarcados()
        {
            return GetResource.GetString("deseja_apagar_recebimento_marcados");
        }

        public string MensagemEntidadeDeletada(RecebimentoCartaoModel e)
        {
            return string.Format(GetResource.GetString("recebimento_deletado_com_sucesso"), e.GetContextMenuHeader);
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return GetResource.GetString("recebimento_nao_deletado");
        }

        public string MensagemEntidadesDeletadas()
        {
            return GetResource.GetString("recebimentos_deletados_com_sucesso");
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return GetResource.GetString("recebimentos_nao_deletados");
        }

        public string TelaApagarCaption()
        {
            return GetResource.GetString("apagar_recebimentos");
        }

        public string MensagemDocumentoDeletado()
        {
            return "LOG de Recebimento de Cartão Marcado Como Deletado Com Sucesso";
        }

        public string MensagemDocumentoNaoDeletado()
        {
            return "Erro ao Marcar LOG de Recebimento de Cartão Como Deletado";
        }

        public string PesquisarEntidadeCaption()
        {
            return "Pesquisa de Recebimento Em Contas";
        }
    }
}

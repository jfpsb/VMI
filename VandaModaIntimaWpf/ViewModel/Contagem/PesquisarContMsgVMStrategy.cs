using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.View.Contagem;
using VandaModaIntimaWpf.View.TipoContagem;
using ContagemModel = VandaModaIntimaWpf.Model.Contagem;

namespace VandaModaIntimaWpf.ViewModel.Contagem
{
    class PesquisarContMsgVMStrategy : IPesquisarMsgVMStrategy<ContagemModel>
    {
        public string MensagemApagarEntidadeCerteza(ContagemModel e)
        {
            return string.Format(GetResource.GetString("certeza_deletar_contagem"), e.GetContextMenuHeader);
        }

        public string MensagemApagarMarcados()
        {
            return GetResource.GetString("deseja_apagar_contagem_marcadas");
        }
        public string MensagemEntidadeDeletada(ContagemModel e)
        {
            return string.Format(GetResource.GetString("contagem_deletada_com_sucesso"), e.GetContextMenuHeader);
        }
        public string MensagemEntidadeNaoDeletada()
        {
            return GetResource.GetString("contagem_nao_deletada");
        }

        public string MensagemEntidadesDeletadas()
        {
            return GetResource.GetString("contagens_deletadas_com_sucesso");
        }
        public string MensagemEntidadesNaoDeletadas()
        {
            return GetResource.GetString("contagens_nao_deletadas");
        }
        public string TelaApagarCaption()
        {
            return GetResource.GetString("apagar_contagens");
        }
        public void AbrirVisualizarContagemProduto(ContagemModel contagem)
        {
            VisualizarContagemProduto visualizar = new VisualizarContagemProduto(contagem);
            visualizar.ShowDialog();
        }
        public void AbrirCadastrarTipoContagem()
        {
            //TODO: Implementar ViewModel
            CadastrarTipoContagem cadastrarTipoContagem = new CadastrarTipoContagem();
            cadastrarTipoContagem.ShowDialog();
        }
        public string MensagemDocumentoDeletado()
        {
            return "LOG de Contagem Marcado Como Deletado Com Sucesso";
        }
        public string MensagemDocumentoNaoDeletado()
        {
            return "Erro ao Marcar LOG de Contagem Como Deletado";
        }
    }
}

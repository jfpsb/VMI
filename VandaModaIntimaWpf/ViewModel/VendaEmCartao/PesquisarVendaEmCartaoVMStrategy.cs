using System;

namespace VandaModaIntimaWpf.ViewModel.VendaEmCartao
{
    public class PesquisarVendaEmCartaoVMStrategy : IPesquisarMsgVMStrategy<Model.VendaEmCartao>
    {
        public string MensagemApagarEntidadeCerteza(Model.VendaEmCartao e)
        {
            throw new NotImplementedException();
        }

        public string MensagemApagarMarcados()
        {
            return "Tem certeza que deseja apagar as vendas em cartão marcadas?";
        }

        public string MensagemEntidadeDeletada(Model.VendaEmCartao e)
        {
            return "Venda em cartão deletada com sucesso.";
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return "Erro ao deletar venda em cartão.";
        }

        public string MensagemEntidadesDeletadas()
        {
            return "Vendas em cartão deletadas com sucesso.";
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return "Erro ao deletar vendas em cartão.";
        }

        public string PesquisarEntidadeCaption()
        {
            return "Vendas em cartão";
        }

        public string TelaApagarCaption()
        {
            return "Deletar venda em cartão";
        }
    }
}

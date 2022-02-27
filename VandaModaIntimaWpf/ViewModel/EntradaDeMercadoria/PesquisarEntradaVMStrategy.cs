using System;

namespace VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria
{
    public class PesquisarEntradaVMStrategy : IPesquisarMsgVMStrategy<Model.EntradaDeMercadoria>
    {
        public string MensagemApagarEntidadeCerteza(Model.EntradaDeMercadoria e)
        {
            return "Tem Certeza Que Deseja Apagar a Entrada De Mercadoria?";
        }

        public string MensagemApagarMarcados()
        {
            return "Tem Certeza Que Deseja Apagar As Entradas De Mercadoria Marcadas?";
        }

        public string MensagemEntidadeDeletada(Model.EntradaDeMercadoria e)
        {
            return "Entrada De Mercadoria Deletado Com Sucesso!";
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return "Erro Ao Deletar Entrada De Mercadoria Deletado!";
        }

        public string MensagemEntidadesDeletadas()
        {
            return "Entradas De Mercadoria Marcadas Deletadas Com Sucesso!";
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return "Erro Ao Deletar Entradas De Mercadoria Marcadas!";
        }

        public string PesquisarEntidadeCaption()
        {
            return "Pesquisa De Entradas De Mercadoria";
        }

        public string TelaApagarCaption()
        {
            return "Apagar Entradas De Mercadoria";
        }
    }
}

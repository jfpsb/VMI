using System;

namespace VandaModaIntimaWpf.ViewModel.Despesa
{
    public class PesquisarDespesaVMStrategy : IPesquisarMsgVMStrategy<Model.Despesa>
    {
        public string MensagemApagarEntidadeCerteza(Model.Despesa e)
        {
            return "Tem Certeza Que Deseja Apagar Esta Despesa?";
        }

        public string MensagemApagarMarcados()
        {
            return "Tem Certeza Que Deseja Apagar As Despesas Marcadas?";
        }

        public string MensagemEntidadeDeletada(Model.Despesa e)
        {
            return "Despesa Foi Apagada Com Sucesso!";
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return "Erro Ao Apagar Despesa!";
        }

        public string MensagemEntidadesDeletadas()
        {
            return "Despesas Marcadas Foram Apagadas Com Suceso!";
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return "Erro Ao Apagar Despesas Marcadas!";
        }

        public string PesquisarEntidadeCaption()
        {
            return "Pesquisa De Despesa";
        }

        public string TelaApagarCaption()
        {
            return "Apagar Despesa";
        }
    }
}

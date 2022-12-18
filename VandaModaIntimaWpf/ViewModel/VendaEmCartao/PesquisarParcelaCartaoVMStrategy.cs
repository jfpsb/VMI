using System;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel.VendaEmCartao
{
    public class PesquisarParcelaCartaoVMStrategy : IPesquisarMsgVMStrategy<Model.ParcelaCartao>
    {
        public string MensagemApagarEntidadeCerteza(ParcelaCartao e)
        {
            throw new NotImplementedException();
        }

        public string MensagemApagarMarcados()
        {
            throw new NotImplementedException();
        }

        public string MensagemEntidadeDeletada(ParcelaCartao e)
        {
            throw new NotImplementedException();
        }

        public string MensagemEntidadeNaoDeletada()
        {
            throw new NotImplementedException();
        }

        public string MensagemEntidadesDeletadas()
        {
            throw new NotImplementedException();
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            throw new NotImplementedException();
        }

        public string PesquisarEntidadeCaption()
        {
            return "Pesquisar parcela de venda em cartão";
        }

        public string TelaApagarCaption()
        {
            throw new NotImplementedException();
        }
    }
}

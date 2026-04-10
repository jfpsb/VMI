using System;
using System.Collections.Generic;
using System.Text;

namespace VandaModaIntimaWpf.ViewModel.Provisionamento
{
    class PesquisarProvisionamentoVMStrategy : IPesquisarMsgVMStrategy<Model.Provisionamento>
    {
        public string MensagemApagarEntidadeCerteza(Model.Provisionamento e)
        {
            return "MensagemApagarEntidadeCerteza";
        }

        public string MensagemApagarMarcados()
        {
            return "MensagemApagarMarcados";
        }

        public string MensagemEntidadeDeletada(Model.Provisionamento e)
        {
            return "MensagemEntidadeDeletada";
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return "MensagemEntidadeNaoDeletada";
        }

        public string MensagemEntidadesDeletadas()
        {
            return "MensagemEntidadesDeletadas";
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return "MensagemEntidadesNaoDeletadas";
        }

        public string PesquisarEntidadeCaption()
        {
            return "PesquisarEntidadeCaption";
        }

        public string TelaApagarCaption()
        {
            return "TelaApagarCaption";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Resources;

namespace VandaModaIntimaWpf.ViewModel.Provisionamento
{
    class PesquisarProvisionamentoVMStrategy : IPesquisarMsgVMStrategy<Model.Provisionamento>
    {
        public string MensagemApagarEntidadeCerteza(Model.Provisionamento e)
        {
            return string.Format(GetResource.GetString("certeza_deletar_provisionamento"), e.GetContextMenuHeader);
        }

        public string MensagemApagarMarcados()
        {
            return GetResource.GetString("deseja_apagar_provisionamentos_marcados");
        }
        public string MensagemEntidadeDeletada(Model.Provisionamento e)
        {
            return string.Format(GetResource.GetString("provisionamento_deletado_com_sucesso"), e.GetContextMenuHeader);
        }
        public string MensagemEntidadeNaoDeletada()
        {
            return GetResource.GetString("provisionamento_nao_deletado");
        }
        public string MensagemEntidadesDeletadas()
        {
            return GetResource.GetString("provisionamentos_deletados_com_sucesso");
        }
        public string MensagemEntidadesNaoDeletadas()
        {
            return GetResource.GetString("provisionamentos_nao_deletados");
        }

        public string PesquisarEntidadeCaption()
        {
            return "Pesquisa de Provisionamentos";
        }

        public string TelaApagarCaption()
        {
            return GetResource.GetString("apagar_provisionamentos");
        }
    }
}

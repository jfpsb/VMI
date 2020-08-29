using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.View.Loja;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    class PesquisarLojaMsgVMStrategy : IPesquisarMsgVMStrategy<LojaModel>
    {
        public string MensagemApagarEntidadeCerteza(LojaModel e)
        {
            return string.Format(GetResource.GetString("certeza_deletar_loja"), e.Nome);
        }

        public string MensagemApagarMarcados()
        {
            return GetResource.GetString("deseja_apagar_loja_marcadas");
        }

        public string MensagemEntidadeDeletada(LojaModel e)
        {
            return string.Format(GetResource.GetString("loja_deletada_com_sucesso"), e.Nome);
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return GetResource.GetString("loja_nao_deletada");
        }

        public string MensagemEntidadesDeletadas()
        {
            return GetResource.GetString("lojas_deletadas_com_sucesso");
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return GetResource.GetString("lojas_nao_deletadas");
        }

        public string TelaApagarCaption()
        {
            return GetResource.GetString("apagar_lojas");
        }

        public string MensagemDocumentoDeletado()
        {
            return "LOG de Loja Marcado Como Deletado Com Sucesso";
        }

        public string MensagemDocumentoNaoDeletado()
        {
            return "Erro ao Marcar LOG de Loja Como Deletado";
        }
    }
}

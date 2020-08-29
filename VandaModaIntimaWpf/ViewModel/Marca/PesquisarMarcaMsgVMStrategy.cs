using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.View.Marca;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.ViewModel.Marca
{
    class PesquisarMarcaMsgVMStrategy : IPesquisarMsgVMStrategy<MarcaModel>
    {
        public string MensagemApagarEntidadeCerteza(MarcaModel e)
        {
            return string.Format(GetResource.GetString("certeza_deletar_marca"), e.Nome);
        }

        public string MensagemApagarMarcados()
        {
            return GetResource.GetString("deseja_apagar_marca_marcadas");
        }

        public string MensagemEntidadeDeletada(MarcaModel e)
        {
            return string.Format(GetResource.GetString("marca_deletada_com_sucesso"), e.Nome);
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return GetResource.GetString("marca_nao_deletada");
        }

        public string MensagemEntidadesDeletadas()
        {
            return GetResource.GetString("marcas_deletadas_com_sucesso");
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return GetResource.GetString("marcas_nao_deletadas");
        }

        public string TelaApagarCaption()
        {
            return GetResource.GetString("apagar_marcas");
        }

        public string MensagemDocumentoDeletado()
        {
            return "LOG de Marca Marcado Como Deletado Com Sucesso";
        }

        public string MensagemDocumentoNaoDeletado()
        {
            return "Erro ao Marcar LOG de Marca Como Deletado";
        }
    }
}

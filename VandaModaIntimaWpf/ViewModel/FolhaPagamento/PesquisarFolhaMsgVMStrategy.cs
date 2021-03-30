using System;
using FolhaPagamentoModel = VandaModaIntimaWpf.Model.FolhaPagamento;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class PesquisarFolhaMsgVMStrategy : IPesquisarMsgVMStrategy<FolhaPagamentoModel>
    {
        public string MensagemApagarEntidadeCerteza(FolhaPagamentoModel e)
        {
            throw new NotImplementedException();
        }
        public string MensagemApagarMarcados()
        {
            throw new NotImplementedException();
        }
        public string MensagemEntidadeDeletada(FolhaPagamentoModel e)
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
        public string TelaApagarCaption()
        {
            throw new NotImplementedException();
        }
        public string MensagemDocumentoDeletado()
        {
            return "LOG de Folha de Pagamento Marcado Como Deletado Com Sucesso";
        }
        public string MensagemDocumentoNaoDeletado()
        {
            return "Erro ao Marcar LOG de Folha de Pagamento Como Deletado";
        }

        public string PesquisarEntidadeCaption()
        {
            return "Consulta de Folha Pagamento";
        }
    }
}

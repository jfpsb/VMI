using FuncionarioModel = VandaModaIntimaWpf.Model.Funcionario;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    class PesquisarFuncMsgVMStrategy : IPesquisarMsgVMStrategy<FuncionarioModel>
    {
        public string MensagemApagarEntidadeCerteza(FuncionarioModel e)
        {
            return "Tem Certeza que Deseja Deletar o Funcionário (a) " + e.Nome + "?";
        }

        public string MensagemApagarMarcados()
        {
            return "Tem Certeza Que Deseja Apagar os Funcionários Marcados?";
        }

        public string MensagemEntidadeDeletada(FuncionarioModel e)
        {
            return "Funcionário " + e.Nome + " Deletado (a) Com Sucesso";
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return "Funcionário Não Foi Deletado";
        }

        public string MensagemEntidadesDeletadas()
        {
            return "Funcionários Marcados Foram Deletados Com Sucesso";
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return "Funcionários Marcados Não Foram Deletados Com Sucesso";
        }

        public string TelaApagarCaption()
        {
            return "Apagar Funcionários";
        }

        public string MensagemDocumentoDeletado()
        {
            return "LOG de Funcionário Marcado Como Deletado Com Sucesso";
        }

        public string MensagemDocumentoNaoDeletado()
        {
            return "Erro ao Marcar LOG de Funcionário Como Deletado";
        }

        public string PesquisarEntidadeCaption()
        {
            return "Pesquisa de Funcionário";
        }
    }
}

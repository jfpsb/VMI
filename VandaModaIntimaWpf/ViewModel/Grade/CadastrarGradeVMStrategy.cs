namespace VandaModaIntimaWpf.ViewModel.Grade
{
    public class CadastrarGradeVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Criação de Grade";
        }

        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Criação de Grade Criado Com Sucesso";
        }

        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Salvar Grade";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Grade Foi Salva Com Sucesso";
        }

        public string MessageBoxCaption()
        {
            return "Cadastro de Grade";
        }
    }
}

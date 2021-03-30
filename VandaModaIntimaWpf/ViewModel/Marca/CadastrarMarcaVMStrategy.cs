namespace VandaModaIntimaWpf.ViewModel.Marca
{
    class CadastrarMarcaVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Crição de Marca Foi Criado Com Sucesso";
        }
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Criação de Marca";
        }
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Inserir Marca";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Marca Foi Inserida Com Sucesso";
        }

        public string MessageBoxCaption()
        {
            return "Cadastro de Marca";
        }
    }
}

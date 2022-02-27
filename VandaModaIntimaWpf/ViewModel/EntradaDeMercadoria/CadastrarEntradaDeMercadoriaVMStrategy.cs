namespace VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria
{
    class CadastrarEntradaDeMercadoriaVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro Ao Cadastrar Entrada De Mercadoria!";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Entrada De Mercadoria Salva Com Sucesso!";
        }

        public string MessageBoxCaption()
        {
            return "Cadastro De Entrada De Mercadoria";
        }
    }
}

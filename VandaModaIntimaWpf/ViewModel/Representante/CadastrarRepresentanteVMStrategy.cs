namespace VandaModaIntimaWpf.ViewModel.Representante
{
    public class CadastrarRepresentanteVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro Ao Cadastrar Representante!";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Representante Cadastrado Com Sucesso!";
        }

        public string MessageBoxCaption()
        {
            return "Cadastro de Representante";
        }
    }
}

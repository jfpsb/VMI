namespace VandaModaIntimaWpf.ViewModel.Representante
{
    public class EditarRepresentanteVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro Ao Salvar Representante!";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Representante Salvo Com Sucesso!";
        }

        public string MessageBoxCaption()
        {
            return "Edição De Representante";
        }
    }
}

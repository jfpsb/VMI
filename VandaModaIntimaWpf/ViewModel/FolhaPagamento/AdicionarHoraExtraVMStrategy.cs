namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarHoraExtraVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro Ao Adicionar Hora Extra";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Sucesso Ao Adicionar Hora Extra";
        }

        public string MessageBoxCaption()
        {
            return "Inserindo Hora Extra";
        }
    }
}

namespace VandaModaIntimaWpf.ViewModel
{
    public interface ICadastrarVMStrategy
    {
        string MensagemDocumentoSalvoComSucesso();
        string MensagemDocumentoNaoSalvo();
        string MensagemEntidadeSalvaComSucesso();
        string MensagemEntidadeErroAoSalvar();
    }
}

using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel
{
    public interface ICadastrarVMStrategy
    {
        string MensagemDocumentoCriadoComSucesso();
        string MensagemDocumentoNaoCriado();
        string MensagemEntidadeInseridaSucesso();
        string MensagemEntidadeErroAoInserir();
        string MensagemDocumentoAtualizadoSucesso();
        string MensagemDocumentoNaoAtualizado();
        string MensagemEntidadeAtualizadaSucesso();
        string MensagemEntidadeNaoAtualizada();
    }
}

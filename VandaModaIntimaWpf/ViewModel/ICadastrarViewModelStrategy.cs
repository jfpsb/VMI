using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel
{
    public interface ICadastrarViewModelStrategy
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

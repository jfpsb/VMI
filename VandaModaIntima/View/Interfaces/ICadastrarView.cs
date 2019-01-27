namespace VandaModaIntima.view.interfaces
{
    interface ICadastrarView
    {
        void MensagemAviso(string mensagem);
        void MensagemErro(string mensagem);
        void LimparCampos();
        void AposCadastro();
    }
}

namespace VandaModaIntimaWpf.ViewModel
{
    /// <summary>
    /// Interface para tornar possível usar MessageBox em View através da ViewModel com ICommand.
    /// Implemente na View.
    /// </summary>
    interface IMessageable
    {
        void MensagemDeAviso(string mensagem);
        void MensagemDeErro(string mensagem);
    }
}

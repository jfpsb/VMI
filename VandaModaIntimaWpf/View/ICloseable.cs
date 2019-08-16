namespace VandaModaIntimaWpf.View
{
    /// <summary>
    /// Interface para tornar possível fechar View através da ViewModel com ICommand.
    /// Implemente essa interface na View, a declaração do método Close já existe na própria classe Window.
    /// </summary>
    interface ICloseable
    {
        void Close();
    }
}

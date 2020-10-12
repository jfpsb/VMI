using VandaModaIntimaWpf.View.Contagem;

namespace VandaModaIntimaWpf.ViewModel.Contagem
{
    /// <summary>
    /// Classe responsável por abrir novas janelas para EditarContagemViewModel .
    /// </summary>
    public static class EditarContagemVMJanela
    {
        //TODO: Colocar essa abertura de janela em um service
        public static void AbrirAdicionarContagemProduto(object viewModel)
        {
            AdicionarContagemProduto adicionarContagemProduto = new AdicionarContagemProduto
            {
                DataContext = viewModel
            };

            adicionarContagemProduto.ShowDialog();
        }
    }
}

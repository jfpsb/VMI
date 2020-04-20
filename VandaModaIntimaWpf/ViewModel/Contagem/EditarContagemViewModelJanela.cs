using VandaModaIntimaWpf.View.Contagem;

namespace VandaModaIntimaWpf.ViewModel.Contagem
{
    /// <summary>
    /// Classe responsável por abrir novas janelas para EditarContagemViewModel .
    /// </summary>
    public static class EditarContagemViewModelJanela
    {
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

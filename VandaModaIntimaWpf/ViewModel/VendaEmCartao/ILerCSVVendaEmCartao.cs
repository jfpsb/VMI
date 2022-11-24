using System.Collections.Generic;

namespace VandaModaIntimaWpf.ViewModel.VendaEmCartao
{
    /// <summary>
    /// Interface para leitura de arquivo .csv disponibilizado pelas operadoras de máquina de cartão.
    /// </summary>
    public interface ILerCSVVendaEmCartao
    {
        IList<Model.VendaEmCartao> GeraListaVendaEmCartao(string caminhoCSV, Model.Loja loja, Model.OperadoraCartao operadora);
    }
}

using System;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoBonusMeta
{
    public interface ICalculaBonusMeta
    {
        /// <summary>
        /// Retorna o valor do bônus que o funcionário receberá baseado na base de cálculo.
        /// </summary>
        /// <param name="totalVendido">Total vendido por funcionário no mês</param>
        /// <param name="valorMeta">Valor de meta de venda do funcionário no mês</param>
        /// <returns></returns>
        double RetornaValorDoBonus(double totalVendido, double valorMeta);

        string DescricaoBonus(DateTime mes);

        string NomeParaDisplay { get; }

        string Descricao { get; }
    }
}

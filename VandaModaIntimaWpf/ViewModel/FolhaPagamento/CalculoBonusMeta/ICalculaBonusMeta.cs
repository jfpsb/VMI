namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoBonusMeta
{
    public interface ICalculaBonusMeta
    {
        /// <summary>
        /// Retorna o valor do bônus que o funcionário receberá baseado na base de cálculo.
        /// </summary>
        /// <param name="baseDeCalculo">Valor da base de cálculo</param>
        /// <returns></returns>
        double RetornaValorDoBonus(double baseDeCalculo);

        /// <summary>
        /// Retorna qual será o valor utilizado como base para calcular o valor que o funcionário receberá como bônus.
        /// </summary>
        /// <param name="totalVendido">Total vendido por mês</param>
        /// <param name="valorMeta">Valor da meta do mês</param>
        /// <returns></returns>
        double RetornaBaseDeCalculo(double totalVendido, double valorMeta);

        string NomeParaDisplay { get; }

        string Descricao { get; }
    }
}

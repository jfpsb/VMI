namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoBonusMeta
{
    /// <summary>
    /// Calcula o bônus de meta com base no total vendido, com alíquota de 0,5% (meio porcento).
    /// </summary>
    public class CalculaBonusMetaMeioPorcento : ICalculaBonusMeta
    {
        public string Descricao => "Calcula o bônus de meta com base no total vendido, com alíquota de 0,5% (meio porcento).";

        public string NomeParaDisplay => "0,5% DO TOTAL VENDIDO";

        public double RetornaValorDoBonus(double baseDeCalculo)
        {
            return baseDeCalculo * 0.005; //0,5%
        }

        public double RetornaBaseDeCalculo(double totalVendido, double valorMeta)
        {
            if (totalVendido >= valorMeta)
                return totalVendido; //Sobre todo o valor vendido.
            return 0.0;
        }
    }
}

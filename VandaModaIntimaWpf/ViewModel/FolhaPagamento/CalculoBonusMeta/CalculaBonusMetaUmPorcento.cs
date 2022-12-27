namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoBonusMeta
{
    public class CalculaBonusMetaUmPorcento : ICalculaBonusMeta
    {
        public string NomeParaDisplay => "1% DO TOTAL VENDIDO";

        public string Descricao => "Calcula o bônus de meta com base no total vendido, com alíquota de 1% (um porcento).";

        public double RetornaValorDoBonus(double baseDeCalculo)
        {
            return baseDeCalculo * 0.01;
        }

        public double RetornaBaseDeCalculo(double totalVendido, double valorMeta)
        {
            if (totalVendido >= valorMeta)
                return totalVendido;
            return 0.0;
        }
    }
}

using System;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoBonusMeta
{
    /// <summary>
    /// Calcula o bônus de meta com base no total vendido após bater a meta, com alíquota de 1% (um porcento).
    /// </summary>
    public class CalculaBonusMetaUmPorcentoAposMeta : ICalculaBonusMeta
    {
        public string NomeParaDisplay => "1% DA DIFERENÇA ENTRE TOTAL VENDIDO E VALOR DA META";

        public string Descricao => "Calcula o bônus de meta com base na diferença entre o total vendido e o valor da meta, com alíquota de 1% (um porcento).";

        public double RetornaValorDoBonus(double baseDeCalculo)
        {
            return baseDeCalculo * 0.01;
        }

        public double RetornaBaseDeCalculo(double totalVendido, double valorMeta)
        {
            double baseCalculo = totalVendido - valorMeta;
            if (baseCalculo <= 0)
                return 0.0;
            return baseCalculo;
        }
    }
}

using System;
using System.Globalization;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoBonusMeta
{
    /// <summary>
    /// Calcula o bônus de meta com base no total vendido após bater a meta, com alíquota de 1% (um porcento).
    /// </summary>
    public class CalculaBonusMetaUmPorcentoAposMeta : ICalculaBonusMeta
    {
        public string NomeParaDisplay => "1% DA DIFERENÇA ENTRE TOTAL VENDIDO E VALOR DA META";

        public string Descricao => "Calcula o bônus de meta com base na diferença entre o total vendido e o valor da meta, com alíquota de 1% (um porcento).";

        public string DescricaoBonus(DateTime mes, double totalVendido, double valorMeta)
        {
            return $"COMISSÃO DE VENDA - 1% DE VALOR ACIMA DA META - {mes.ToString("MMMM", CultureInfo.GetCultureInfo("pt-BR"))}";
        }

        public double RetornaValorDoBonus(double totalVendido, double valorMeta)
        {
            if (valorMeta == 0 || totalVendido == 0) return 0;
            double baseDeCalculo = 0;
            if (totalVendido > valorMeta) baseDeCalculo = totalVendido - valorMeta;
            return baseDeCalculo * 0.01; //1%
        }
    }
}

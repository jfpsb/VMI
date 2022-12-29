using System;
using System.Globalization;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoBonusMeta
{
    /// <summary>
    /// Calcula o bônus de meta com base no total vendido, com alíquota de 0,5% (meio porcento).
    /// </summary>
    public class CalculaBonusMetaMeioPorcento : ICalculaBonusMeta
    {
        public string Descricao => "Calcula o bônus de meta com base no total vendido, com alíquota de 0,5% (meio porcento).";

        public string NomeParaDisplay => "0,5% DO TOTAL VENDIDO";

        public string DescricaoBonus(DateTime mes, double totalVendido, double valorMeta)
        {
            return $"COMISSÃO DE VENDA - 0,5% - {mes.ToString("MMMM", CultureInfo.GetCultureInfo("pt-BR"))}";
        }

        public double RetornaValorDoBonus(double totalVendido, double valorMeta)
        {
            if (valorMeta == 0 || totalVendido == 0) return 0;
            double baseDeCalculo = 0;
            if (totalVendido >= valorMeta) baseDeCalculo = totalVendido;
            return baseDeCalculo * 0.005; //0,5%
        }
    }
}

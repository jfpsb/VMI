using System.Globalization;
using System;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoBonusMeta
{
    public class CalculaBonusMetaUmPorcento : ICalculaBonusMeta
    {
        public string NomeParaDisplay => "1% DO TOTAL VENDIDO";

        public string Descricao => "Calcula o bônus de meta com base no total vendido, com alíquota de 1% (um porcento).";

        public string DescricaoBonus(DateTime mes)
        {
            return $"COMISSÃO DE VENDA - 1% - {mes.ToString("MMMM", CultureInfo.GetCultureInfo("pt-BR"))}";
        }

        public double RetornaValorDoBonus(double totalVendido, double valorMeta)
        {
            if (valorMeta == 0 || totalVendido == 0) return 0;
            double baseDeCalculo = 0;
            if (totalVendido >= valorMeta) baseDeCalculo = totalVendido;
            return baseDeCalculo * 0.01; //1%
        }
    }
}

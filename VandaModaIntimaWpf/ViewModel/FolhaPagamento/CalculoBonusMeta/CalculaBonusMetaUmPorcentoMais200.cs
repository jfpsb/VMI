using System;
using System.Globalization;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoBonusMeta
{
    public class CalculaBonusMetaUmPorcentoMais200 : ICalculaBonusMeta
    {
        public string NomeParaDisplay => "1% DO TOTAL VENDIDO + R$ 200,00";

        public string Descricao => "Calcula o bônus de meta com base no total vendido. Quem não bater a meta recebe 1% sobre o total vendido. Quem bater a meta recebe R$ 200,00 de bônus adicional além dos 1%.";

        public string DescricaoBonus(DateTime mes, double totalVendido, double valorMeta)
        {
            if (totalVendido >= valorMeta)
            {
                return $"COMISSÃO DE VENDA - 1% (+R$200,00 se bater a meta) - {mes.ToString("MMMM", CultureInfo.GetCultureInfo("pt-BR"))}";
            }

            return $"COMISSÃO DE VENDA - 1% - {mes.ToString("MMMM", CultureInfo.GetCultureInfo("pt-BR"))}";
        }

        public double RetornaValorDoBonus(double totalVendido, double valorMeta)
        {
            if (valorMeta == 0 || totalVendido == 0) return 0;

            double baseDeCalculo = totalVendido;
            double valorBonusMeta = 0;

            if (totalVendido >= valorMeta)
            {
                valorBonusMeta = 200.0;
            }

            return (baseDeCalculo * 0.01) + valorBonusMeta; //1% (+ 200,00, se bater a meta)
        }
    }
}

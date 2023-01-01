using System;
using System.Globalization;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoBonusMeta
{
    public class CalculaBonusMetaFinalDeAno : ICalculaBonusMeta
    {
        public string NomeParaDisplay => "SE META BATIDA 1% + R$200,00; SENÃO R$150,00. INCLUI GRATIFICAÇÃO";

        public string Descricao => "Se vendedor bateu a meta de venda individual, recebe 1% do valor total vendido + R$ 200,00; senão, recebe R$ 150,00 como gratificação. Gratifica R$150,00 para funcionários em outras funções. Utilizado somente no mês de dezembro.";

        public string DescricaoBonus(DateTime mes, double totalVendido, double valorMeta)
        {
            if (valorMeta > 0 && totalVendido >= valorMeta)
            {
                return $"COMISSÃO DE VENDA - 1% + R$200,00 - {mes.ToString("MMMM", CultureInfo.GetCultureInfo("pt-BR"))}";
            }

            return $"GRATIFICAÇÃO NATAL";
        }

        public double RetornaValorDoBonus(double totalVendido, double valorMeta)
        {
            double valorBonusMeta = 150; //Valor padrão

            if (totalVendido > 0 && valorMeta > 0) //Vendedor
            {
                double baseDeCalculo = 0;

                if (totalVendido >= valorMeta)
                {
                    valorBonusMeta = 200.0;
                    baseDeCalculo = totalVendido;
                }

                return valorBonusMeta + (baseDeCalculo * 0.01); //1% + 200,00
            }

            //Outras funções
            return valorBonusMeta;
        }
    }
}

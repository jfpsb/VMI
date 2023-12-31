using System;
using System.Globalization;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoBonusMeta
{
    public class CalculoBonusMetaProporcionalMaxUmPorcento : ICalculaBonusMeta
    {
        public string NomeParaDisplay => "PROPORCIONAL AO TOTAL VENDIDO COM TETO DE 1%. COM GRATIFICAÇÃO DE 150,00.";

        public string Descricao => "O percentual utilizado é a proporção entre total vendido e meta de venda. Com teto de 1%. Gratificação de R$ 150,00 para os demais funcionários ou quem ficou abaixo do valor da gratificação.";

        public string DescricaoBonus(DateTime mes, double totalVendido, double valorMeta)
        {
            if (valorMeta > 0 && totalVendido > 0)
            {
                var bonus = totalVendido * (totalVendido / valorMeta / 100);
                var porcentagem = totalVendido / valorMeta;
                if(porcentagem > 1.0) porcentagem = 1.0;
                if (bonus > 150.0)
                {
                    return $"COMISSÃO DE VENDA - {Math.Round(porcentagem, 2)}% DO TOTAL VENDIDO - {mes.ToString("MMMM", CultureInfo.GetCultureInfo("pt-BR"))}";
                }
            }

            return $"GRATIFICAÇÃO NATAL";
        }

        public double RetornaValorDoBonus(double totalVendido, double valorMeta)
        {
            double pisoBonusMeta = 150; //Valor padrão

            if (totalVendido > 0 && valorMeta > 0) //Vendedor
            {
                double baseDeCalculo = totalVendido;

                if (totalVendido >= valorMeta)
                {
                    return baseDeCalculo * 0.01; //1%
                }

                var bonus = baseDeCalculo * (totalVendido / valorMeta / 100);

                if (bonus > pisoBonusMeta)
                    return bonus;
            }

            return pisoBonusMeta;    
        }
    }
}

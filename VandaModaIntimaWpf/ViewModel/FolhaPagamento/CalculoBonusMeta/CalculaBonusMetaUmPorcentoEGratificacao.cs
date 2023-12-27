using System;
using System.Globalization;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoBonusMeta
{
    public class CalculaBonusMetaUmPorcentoEGratificacao : ICalculaBonusMeta
    {
        public string NomeParaDisplay => "SE VENDEDOR BATER SUA META RECEBE 1% SOBRE VALOR VENDIDO. GRATIFICAÇÃO DE R$ 150,00 PARA RESTANTE.";

        public string Descricao => "Se vendedor bateu a meta de venda individual, recebe 1% do valor total vendido; senão, recebe R$ 150,00 como gratificação. Gratifica R$150,00 para funcionários em outras funções. Utilizado somente no mês de dezembro.";

        public string DescricaoBonus(DateTime mes, double totalVendido, double valorMeta)
        {
            if (valorMeta > 0 && totalVendido >= valorMeta)
            {
                return $"COMISSÃO DE VENDA - 1% - {mes.ToString("MMMM", CultureInfo.GetCultureInfo("pt-BR"))}";
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
                    baseDeCalculo = totalVendido;
                }

                return baseDeCalculo * 0.01; //1%
            }

            return valorBonusMeta;
        }
    }
}

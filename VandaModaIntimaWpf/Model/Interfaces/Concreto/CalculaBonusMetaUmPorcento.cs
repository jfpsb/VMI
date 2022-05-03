using System;

namespace VandaModaIntimaWpf.Model.Interfaces.Concreto
{
    /// <summary>
    /// Calcula o bônus de meta com base no total vendido após bater a meta, com alíquota de 1% (um porcento).
    /// </summary>
    public class CalculaBonusMetaUmPorcento : ICalculaBonusMeta
    {
        public double BonusDeMeta(double baseDeCalculo)
        {
            return baseDeCalculo * 0.01;
        }

        public double CalculaBaseDeCalculo(double totalVendido, double valorMeta)
        {
            double baseCalculo = totalVendido - valorMeta;
            if (baseCalculo <= 0)
                return 0.0;
            return baseCalculo;
        }
    }
}

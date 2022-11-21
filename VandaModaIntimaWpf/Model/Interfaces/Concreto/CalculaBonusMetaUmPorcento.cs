namespace VandaModaIntimaWpf.Model.Interfaces.Concreto
{
    public class CalculaBonusMetaUmPorcento : ICalculaBonusMeta
    {
        public double BonusDeMeta(double baseDeCalculo)
        {
            return baseDeCalculo * 0.01;
        }

        public double CalculaBaseDeCalculo(double totalVendido, double valorMeta)
        {
            if (totalVendido >= valorMeta)
                return totalVendido;
            return 0.0;
        }
    }
}

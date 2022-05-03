namespace VandaModaIntimaWpf.Model.Interfaces.Concreto
{
    /// <summary>
    /// Calcula o bônus de meta com base no total vendido, com alíquota de 0,5% (meio porcento).
    /// </summary>
    public class CalculaBonusMetaMeioPorcento : ICalculaBonusMeta
    {
        public double BonusDeMeta(double baseDeCalculo)
        {
            return baseDeCalculo * 0.005;
        }

        public double CalculaBaseDeCalculo(double totalVendido, double valorMeta)
        {
            if (totalVendido >= valorMeta)
                return totalVendido;
            return 0.0;
        }
    }
}

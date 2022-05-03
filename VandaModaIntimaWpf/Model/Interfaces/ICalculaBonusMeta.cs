namespace VandaModaIntimaWpf.Model.Interfaces
{
    public interface ICalculaBonusMeta
    {
        double BonusDeMeta(double baseDeCalculo);
        double CalculaBaseDeCalculo(double totalVendido, double valorMeta);
    }
}

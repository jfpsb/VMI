using System;

namespace VandaModaIntimaWpf.View.Interfaces.DataUpDown
{
    public class SomaAno : IDataUpDown
    {
        public DateTime Somar(DateTime data)
        {
            return data.AddYears(1);
        }

        public DateTime Subtrair(DateTime data)
        {
            return data.AddYears(-1);
        }
    }
}

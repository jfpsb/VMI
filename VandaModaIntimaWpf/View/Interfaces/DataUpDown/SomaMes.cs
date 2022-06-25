using System;

namespace VandaModaIntimaWpf.View.Interfaces.DataUpDown
{
    public class SomaMes : IDataUpDown
    {
        public DateTime Somar(DateTime data)
        {
            if (data.Month == 12)
            {
                data = data.AddMonths(-11);
                data = data.AddYears(1);
            }
            else
            {
                data = data.AddMonths(1);
            }

            return data;
        }

        public DateTime Subtrair(DateTime data)
        {
            if (data.Month == 1)
            {
                data = data.AddMonths(11);
                data = data.AddYears(-1);
            }
            else
            {
                data = data.AddMonths(-1);
            }

            return data;
        }
    }
}

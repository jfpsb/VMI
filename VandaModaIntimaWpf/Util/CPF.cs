using System;
using System.Linq;

namespace VandaModaIntimaWpf.Util
{
    public static class CPF
    {
        public static bool IsValid(string cpf)
        {
            if (cpf.Length != 11)
                return false;

            //Todos os caracteres são iguais
            if (cpf.Distinct().Count() == 1)
                return false;

            int[] cpfArray = Array.ConvertAll(cpf.ToCharArray(), s => int.Parse(s.ToString()));
            int somaValidacao = 0;

            for (int i = 2; i <= 10; i++)
            {
                somaValidacao += cpfArray[10 - i] * i;
            }

            var resto = somaValidacao * 10 % 11;
            if (resto == 10) resto = 0;

            if (resto != cpfArray[9])
                return false;

            somaValidacao = 0;

            for (int i = 2; i <= 11; i++)
            {
                somaValidacao += cpfArray[11 - i] * i;
            }

            resto = somaValidacao * 10 % 11;
            if (resto == 10) resto = 0;

            if (resto != cpfArray[10])
                return false;

            return true;
        }
    }
}

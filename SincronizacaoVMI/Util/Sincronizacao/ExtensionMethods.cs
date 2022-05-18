using NHibernate.Type;
using System;
using System.Linq;

namespace SincronizacaoVMI.Util.Sincronizacao
{
    public static class ExtensionMethods
    {
        public static bool ContainsType(this IType[] types, Type tipo)
        {
            return types.Any(a => a.GetType().Equals(tipo));
        }

        public static int PropIndex(this string[] props, string nome)
        {
            int i;
            for (i = 0; i < props.Length; i++)
            {
                if (props[i].Equals(nome))
                    return i;
            }
            return -1;
        }
    }
}

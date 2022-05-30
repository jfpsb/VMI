using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.ViewModel.Util
{
    public sealed class ViewModelParameterHandler
    {
        static readonly ViewModelParameterHandler instance = new ViewModelParameterHandler();

        /// <summary>
        /// Tupla consiste em uma string com o nome do parâmetro e um objeto com valor.
        /// </summary>
        Dictionary<Type, Dictionary<string, object>> parameterDictionary = new Dictionary<Type, Dictionary<string, object>>();

        public static ViewModelParameterHandler Instance
        {
            get { return instance; }
        }

        public void AdicionaParametros(Type tipoVM, Dictionary<string, object> paramValue)
        {
            parameterDictionary.Add(tipoVM, paramValue);
        }

        public Dictionary<string, object> GetParametros(Type tipoVM)
        {
            if (parameterDictionary.ContainsKey(tipoVM))
            {
                return parameterDictionary[tipoVM];
            }
            return null;
        }
    }
}

using System;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.ViewModel.Util
{
    public enum ViewModelMessages
    {
        ProdutoSalvo = 1,
        LojaSalva = 2,
        FornecedorSalvo = 3,
        FuncionarioSalvo = 4
    };
    public sealed class Mediator
    {
        static readonly Mediator instance = new Mediator();

        MultiDictionary<ViewModelMessages, Action<object>> internalList = new MultiDictionary<ViewModelMessages, Action<Object>>();

        static Mediator()
        {

        }
        private Mediator()
        {

        }

        public static Mediator Instance { get { return instance; } }

        public void Registrar(ViewModelMessages message, Action<object> action)
        {
            internalList.AddValue(message, action);
        }

        public void Notificar(ViewModelMessages message, object args)
        {
            if (internalList.ContainsKey(message))
            {
                foreach (Action<object> action in internalList[message])
                {
                    action(args);
                }
            }
        }
    }
}

using System.Collections.Generic;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    public class WorksheetContainer<T> where T : class
    {
        private string _nome;
        private IList<T> _lista;
        private int _tamanhoFonteGeral = 12;
        public string Nome { get => _nome; set => _nome = value; }
        public IList<T> Lista { get => _lista; set => _lista = value; }
        public int TamanhoFonteGeral { get => _tamanhoFonteGeral; }
    }
}

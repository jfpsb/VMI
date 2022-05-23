using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.CrystalReports
{
    public interface ICrystalReportsParaPDF<E> where E : AModel
    {
        Task Salvar(string caminhoPasta,
            IList<E> lista,
            IProgress<double> valorBarraProgresso,
            IProgress<string> descricaoBarraProgresso,
            IProgress<bool> isIndeterminadaBarraProgresso,
            CancellationToken token);
    }
}

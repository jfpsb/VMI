using System.Collections.Generic;
using System.Windows.Forms;

namespace VandaModaIntima.view.interfaces
{
    interface IPesquisarView
    {
        void Mensagem(string mensagem);
        void ExportarParaExcel(DataGridView dataGridView);
        void AtribuiDataSource<T>(IList<T> lista);
        void RealizarPesquisa();
    }
}

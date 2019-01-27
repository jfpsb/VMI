using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace VandaModaIntima.view.interfaces
{
    interface IPesquisarView
    {
        void Mensagem(string mensagem);
        void ExportarParaExcel(DataGridView dataGridView);
        void AtribuiDataSource(IList lista);
    }
}

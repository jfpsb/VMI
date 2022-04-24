using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VandaModaIntimaWpf.View.Controles
{
    public class DataGridDespesaGroupByDescricao : DataGrid
    {
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            Columns.Clear();

            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Width = DataGridLength.Auto;
            dataGridTextColumn.Header = "João Paulo 1";

            Columns.Add(dataGridTextColumn);

            base.OnItemsSourceChanged(oldValue, newValue);
        }
    }
}

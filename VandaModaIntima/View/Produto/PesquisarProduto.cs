using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VandaModaIntima.view.interfaces;

namespace VandaModaIntima.View.Produto
{
    public partial class PesquisarProduto : TelaDePesquisa, IPesquisarView
    {
        public PesquisarProduto()
        {
            InitializeComponent();
        }

        public void AtribuiDataSource(IList lista)
        {
            throw new NotImplementedException();
        }

        public void ExportarParaExcel(DataGridView dataGridView)
        {
            throw new NotImplementedException();
        }

        public void Mensagem(string mensagem)
        {
            MessageBox.Show(mensagem);
        }

        private void cadastrarNovoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}

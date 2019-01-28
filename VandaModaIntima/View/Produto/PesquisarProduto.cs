using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VandaModaIntima.Controller.Produto;
using VandaModaIntima.view.interfaces;

namespace VandaModaIntima.View.Produto
{
    public partial class PesquisarProduto : Form, IPesquisarView
    {
        private PesquisarProdutoController pesquisarProdutoController;

        public PesquisarProduto()
        {
            InitializeComponent();
        }

        private void PesquisarProduto_Load(object sender, EventArgs e)
        {
            pesquisarProdutoController = new PesquisarProdutoController(this);
            cmbPesquisarPor.SelectedIndex = 0;
            dgvProduto.AutoGenerateColumns = false;
        }

        public void AtribuiDataSource<Produto>(IList<Produto> lista)
        {
            dgvProduto.DataSource = lista;
        }

        public void ExportarParaExcel(DataGridView dataGridView)
        {
            throw new NotImplementedException();
        }

        public void Mensagem(string mensagem)
        {
            MessageBox.Show(mensagem);
        }

        private void CadastrarNovoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CadastrarProduto().Show();
        }

        private void PesquisarProduto_FormClosing(object sender, FormClosingEventArgs e)
        {
            pesquisarProdutoController.FechandoTela();
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            pesquisarProdutoController.PesquisarPorDescricao(TxtPesquisa.Text);
        }
    }
}

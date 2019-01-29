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

        private enum Pesquisa
        {
            DESCRICAO,
            CODIGO,
            FORNECEDOR,
            MARCA
        }

        public PesquisarProduto()
        {
            InitializeComponent();
        }

        private void PesquisarProduto_Load(object sender, EventArgs e)
        {
            pesquisarProdutoController = new PesquisarProdutoController(this);
            dgvProduto.AutoGenerateColumns = false; //Tem que ficar antes do SelectIndex do ComboBox (confia)
            CmbPesquisarPor.SelectedIndex = 0;
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

        private void TxtPesquisa_TextChanged(object sender, EventArgs e)
        {
            RealizarPesquisa();
        }

        public void RealizarPesquisa()
        {
            switch (CmbPesquisarPor.SelectedIndex)
            {
                case (int)Pesquisa.DESCRICAO:
                    pesquisarProdutoController.PesquisarPorDescricao(TxtPesquisa.Text);
                    break;

                case (int)Pesquisa.CODIGO:
                    pesquisarProdutoController.PesquisarPorCodigoDeBarra(TxtPesquisa.Text);
                    break;

                case (int)Pesquisa.FORNECEDOR:
                    pesquisarProdutoController.PesquisarPorFornecedor(TxtPesquisa.Text);
                    break;

                case (int)Pesquisa.MARCA:
                    pesquisarProdutoController.PesquisarPorMarca(TxtPesquisa.Text);
                    break;
            }
        }

        private void CmbPesquisarPor_SelectedIndexChanged(object sender, EventArgs e)
        {
            RealizarPesquisa();
        }
    }
}

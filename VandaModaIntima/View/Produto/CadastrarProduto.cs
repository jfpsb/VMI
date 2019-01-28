using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VandaModaIntima.Controller.Produto;
using VandaModaIntima.Model;
using VandaModaIntima.view.interfaces;

namespace VandaModaIntima.View.Produto
{
    public partial class CadastrarProduto : Form, ICadastrarView
    {
        private CadastrarProdutoController cadastrarProdutoController;

        public CadastrarProduto()
        {
            InitializeComponent();
        }

        private void CadastrarProduto_Load(object sender, EventArgs e)
        {
            cadastrarProdutoController = new CadastrarProdutoController(this);
            cadastrarProdutoController.PesquisarFornecedor();
            cadastrarProdutoController.PesquisarMarca();
        }

        public void AposCadastro()
        {
            throw new NotImplementedException();
        }

        public void LimparCampos()
        {
            TxtCodigoBarra.Clear();
            TxtDescricao.Clear();
            TxtPreco.Clear();
            CmbFornecedor.Text = "";
            CmbMarca.Text = "";
        }

        public void MensagemAviso(string mensagem)
        {
            MessageBox.Show(mensagem, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void MensagemErro(string mensagem)
        {
            MessageBox.Show(mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void BtnCadastrar_Click(object sender, EventArgs e)
        {

        }

        private void CmbFornecedor_TextChanged(object sender, EventArgs e)
        {
            if (CmbFornecedor.Text != string.Empty)
            {
                //cadastrarProdutoController.PesquisarFornecedor(CmbFornecedor.Text);
                //CmbFornecedor.SelectionStart = CmbFornecedor.Text.Length;
                //CmbFornecedor.Focus();
                //CmbFornecedor.DroppedDown = true;
            }
            else
            {
                //CmbFornecedor.DroppedDown = false;
            }
        }

        private void CmbMarca_TextChanged(object sender, EventArgs e)
        {
            if (CmbMarca.Text != string.Empty)
            {

            }
        }

        public void AtribuiDataSourceCmbMarca(IList<Marca> marcas)
        {
            CmbMarca.DataSource = marcas;
        }

        public void AtribuiDataSourceCmbFornecedor(IList<Fornecedor> fornecedores)
        {
            CmbFornecedor.DataSource = fornecedores;
        }
    }
}

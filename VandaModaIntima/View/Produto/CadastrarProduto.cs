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
        private Fornecedor fornecedor = null;
        private Marca marca = null;
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
            
        }

        public void LimparCampos()
        {
            TxtCodigoBarra.Clear();
            TxtDescricao.Clear();
            TxtPreco.Clear();
            fornecedor = null;
            marca = null;
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
            if(CmbFornecedor.SelectedIndex != 0)
            {
                fornecedor = CmbFornecedor.SelectedItem as Fornecedor;
            }

            if(CmbMarca.SelectedIndex != 0)
            {
                marca = CmbMarca.SelectedItem as Marca;
            }

            cadastrarProdutoController.Cadastrar(TxtCodigoBarra.Text, fornecedor, marca, TxtDescricao.Text, TxtPreco.Text);
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

using System;
using System.Windows.Forms;
using VandaModaIntima.view.interfaces;

namespace VandaModaIntima.View.Produto
{
    public partial class CadastrarProduto : Form, ICadastrarView
    {
        public CadastrarProduto()
        {
            InitializeComponent();
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
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VandaModaIntima.BancoDeDados.ConnectionFactory;
using VandaModaIntima.View.Produto;

namespace VandaModaIntima
{
    public partial class TelaInicial : Form
    {
        public TelaInicial()
        {
            InitializeComponent();
        }

        private void TelaInicial_Load(object sender, EventArgs e)
        {
            ConexaoBanco();
        }

        private void BtnProduto_Click(object sender, EventArgs e)
        {
            PesquisarProduto pesquisarProduto = new PesquisarProduto();
            pesquisarProduto.Show();
        }

        private void TelaInicial_FormClosing(object sender, FormClosingEventArgs e)
        {
            SessionProvider.FechaConexoes();
        }

        private void ConexaoBanco()
        {
            try
            {
                SessionProvider.MySessionFactory = SessionProvider.GetMySession().SessionFactory;
            }
            catch (NullReferenceException nre)
            {
                MessageBox.Show("Houve um erro ao conectar ao banco de dados.\n\n" + nre.Message);
            }
        }
    }
}

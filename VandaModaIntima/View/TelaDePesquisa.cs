using NHibernate;
using System.ComponentModel;
using System.Windows.Forms;
using VandaModaIntima.BancoDeDados.ConnectionFactory;

namespace VandaModaIntima.View
{
    public class TelaDePesquisa : Form
    {
        protected ISession sessao;

        public TelaDePesquisa()
        {
            FormClosing += new FormClosingEventHandler(FechandoTela);
        }

        private void FechandoTela(object sender, CancelEventArgs e)
        {
            SessionProvider.FechaSession(sessao);
        }
    }
}

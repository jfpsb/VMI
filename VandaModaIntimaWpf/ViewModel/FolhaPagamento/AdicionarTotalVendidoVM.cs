using NHibernate;
using System.ComponentModel;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.View.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarTotalVendidoVM : ACadastrarViewModel<Model.FolhaPagamento>
    {
        public AdicionarTotalVendidoVM(ISession session, Model.FolhaPagamento folha) : base(session, false)
        {
            daoEntidade = new DAOFolhaPagamento(_session);
            viewModelStrategy = new AdicionarTotalVendidoVMStrategy();
            Entidade = folha;
            AposInserirNoBancoDeDados += FecharTela;
        }

        private void FecharTela(AposCRUDEventArgs e)
        {
            if (e.Sucesso)
            {
                if (e.Parametro is ICloseable closeable)
                {
                    closeable.Close();
                }
            }
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        public override void ResetaPropriedades(AposCRUDEventArgs e)
        {
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (Entidade.TotalVendido < 0.0)
            {
                BtnSalvarToolTip += "Informe Um Valor De Total Vendido Válido!\n";
                valido = false;
            }

            return valido;
        }
    }
}

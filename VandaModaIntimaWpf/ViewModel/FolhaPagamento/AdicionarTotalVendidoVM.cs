using NHibernate;
using System.ComponentModel;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarTotalVendidoVM : ACadastrarViewModel<Model.FolhaPagamento>
    {
        public AdicionarTotalVendidoVM(ISession session, Model.FolhaPagamento folha, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            _session = session;
            daoEntidade = new DAOFolhaPagamento(session);
            viewModelStrategy = new AdicionarTotalVendidoVMStrategy();
            Entidade = folha;
            AposInserirNoBancoDeDados += FecharTela;
        }

        private void FecharTela(AposInserirBDEventArgs e)
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

        public override void ResetaPropriedades(AposInserirBDEventArgs e)
        {
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (Entidade.TotalVendido <= 0.0)
            {
                BtnSalvarToolTip += "Informe Um Valor De Total Vendido Válido!\n";
                valido = false;
            }

            return valido;
        }
    }
}

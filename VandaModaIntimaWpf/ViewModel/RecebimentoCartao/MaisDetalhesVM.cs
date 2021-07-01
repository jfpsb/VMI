using NHibernate;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO.MySQL;

namespace VandaModaIntimaWpf.ViewModel.RecebimentoCartao
{
    public class MaisDetalhesVM : ObservableObject
    {
        public ObservableCollection<Model.RecebimentoCartao> Recebimentos { get; set; }

        private DAORecebimentoCartao daoRecebimentoCartao;
        private Model.RecebimentoCartao _recebimento;

        public MaisDetalhesVM(ISession session, Model.RecebimentoCartao recebimentoCartao)
        {
            Recebimento = recebimentoCartao;
            daoRecebimentoCartao = new DAORecebimentoCartao(session);

            GetRecebimentos();
        }

        private async Task GetRecebimentos()
        {
            Recebimentos = new ObservableCollection<Model.RecebimentoCartao>(await daoRecebimentoCartao.ListarPorMesAnoLoja(Recebimento.Mes, Recebimento.Ano, Recebimento.Loja));
        }

        public Model.RecebimentoCartao Recebimento
        {
            get => _recebimento;
            set
            {
                _recebimento = value;
                OnPropertyChanged("Recebimento");
            }
        }
    }
}

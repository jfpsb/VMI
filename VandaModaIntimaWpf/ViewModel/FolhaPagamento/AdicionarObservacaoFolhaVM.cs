using NHibernate;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarObservacaoFolhaVM : ObservableObject
    {
        private ISession _session;
        private Model.FolhaPagamento _folha;
        private DAOFolhaPagamento daoFolha;
        private IMessageBoxService _messageBoxService;

        public ICommand SalvarComando { get; set; }

        public AdicionarObservacaoFolhaVM(ISession session, Model.FolhaPagamento folha, IMessageBoxService messageBoxService)
        {
            _session = session;
            Folha = folha;
            _messageBoxService = messageBoxService;

            daoFolha = new DAOFolhaPagamento(session);

            SalvarComando = new RelayCommand(Salvar, Validacao);
        }

        private bool Validacao(object arg)
        {
            if (Folha.Observacao?.Trim().Length == 0)
                return false;

            return true;
        }

        private async void Salvar(object obj)
        {
            var result = await daoFolha.InserirOuAtualizar(Folha);

            if (result)
            {
                _messageBoxService.Show("Observação Foi Adicionada Com Sucesso!", "Adicionar Observação Em Folha De Pagamento");
            }
            else
            {
                _messageBoxService.Show("Erro Ao Adicionar Observação!", "Adicionar Observação Em Folha De Pagamento");
            }
        }

        public Model.FolhaPagamento Folha
        {
            get => _folha;
            set
            {
                _folha = value;
                OnPropertyChanged("Folha");
            }
        }
    }
}

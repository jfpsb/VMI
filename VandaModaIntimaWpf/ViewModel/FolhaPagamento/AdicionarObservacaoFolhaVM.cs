using NHibernate;
using System;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarObservacaoFolhaVM : ObservableObject
    {
        private ISession _session;
        private Model.FolhaPagamento _folha;
        private DAOFolhaPagamento daoFolha;
        private IMessageBoxService messageBoxService;

        public ICommand SalvarComando { get; set; }

        public AdicionarObservacaoFolhaVM(ISession session, Model.FolhaPagamento folha, IMessageBoxService messageBoxService)
        {
            _session = session;
            Folha = folha;
            this.messageBoxService = messageBoxService;

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
            try
            {
                await daoFolha.InserirOuAtualizar(Folha);
                messageBoxService.Show("Observação foi adicionada com sucesso.", "Adicionar Observação Em Folha De Pagamento",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                messageBoxService.Show($"Erro ao adicionar observação. Para mais detalhes acesse {Log.LogBanco}.\n\n" +
                    $"{ex.Message}\n\n{ex.InnerException.Message}", "Adicionar Observação Em Folha De Pagamento");
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

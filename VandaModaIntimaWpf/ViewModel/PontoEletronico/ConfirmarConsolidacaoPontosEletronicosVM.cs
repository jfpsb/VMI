using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.ViewModel.Interfaces;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.PontoEletronico
{
    public class ConfirmarConsolidacaoPontosEletronicosVM : ObservableObject, IRequestClose
    {
        private ObservableCollection<object> _listaConsolidacao;
        private Model.Funcionario _funcionario;
        private ISession _session;
        private IMessageBoxService messageBoxService;

        public event EventHandler<EventArgs> RequestClose;

        public ICommand ConfirmarComando { get; set; }

        public ConfirmarConsolidacaoPontosEletronicosVM(ISession session, IList<object> listaConsolidacao)
        {
            _session = session;
            ListaConsolidacao = new ObservableCollection<object>(listaConsolidacao);
            messageBoxService = new MessageBoxService();
            ConfirmarComando = new RelayCommand(Confirmar);
        }

        private async void Confirmar(object obj)
        {
            try
            {
                await InserirListaConsolidacao();
                messageBoxService.Show("Faltas/Horas extras foram registradas com sucesso.", "Confirmação de Pontos Eletrônicos",
                     System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                messageBoxService.Show($"Erro ao registras faltas/horas extras.\n\n{ex.Message}", "Confirmação de Pontos Eletrônicos",
                     System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private async Task InserirListaConsolidacao()
        {
            using (ITransaction tx = _session.BeginTransaction())
            {
                try
                {
                    foreach (var item in ListaConsolidacao)
                    {
                        await _session.SaveAsync(item);
                    }

                    await tx.CommitAsync();
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    Log.EscreveLogBanco(ex, "inserir lista metodo estático em banco de dados");
                    throw new Exception($"Erro ao inserir lista em banco de dados. Acesse {Log.LogBanco} para mais detalhes", ex);
                }
            }
        }

        public ObservableCollection<object> ListaConsolidacao
        {
            get
            {
                return _listaConsolidacao;
            }

            set
            {
                _listaConsolidacao = value;
                OnPropertyChanged("ListaConsolidacao");
            }
        }

        public Model.Funcionario Funcionario
        {
            get
            {
                return _funcionario;
            }

            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
            }
        }
    }
}

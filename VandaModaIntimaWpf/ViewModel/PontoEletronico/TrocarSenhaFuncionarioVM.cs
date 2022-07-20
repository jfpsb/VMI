using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Interfaces;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using VandaModaIntimaWpf.ViewModel.Util;

namespace VandaModaIntimaWpf.ViewModel.PontoEletronico
{
    public class TrocarSenhaFuncionarioVM : ObservableObject, IRequestClose
    {
        private string _senhaAtual;
        private string _novaSenha;
        private Model.Funcionario _funcionario;
        private IMessageBoxService messageBoxService;
        private DAOFuncionario daoFuncionario;
        private bool _possuiSenhaAtual;

        public event EventHandler<EventArgs> RequestClose;
        public ICommand ConfirmarComando { get; set; }

        public TrocarSenhaFuncionarioVM(ISession session, Model.Funcionario funcionario, IMessageBoxService messageBoxService)
        {
            Funcionario = funcionario;
            this.messageBoxService = messageBoxService;
            daoFuncionario = new DAOFuncionario(session);

            PossuiSenhaAtual = Funcionario.Senha != null && Funcionario.Senha.Length != 0;
            if (!PossuiSenhaAtual)
            {
                messageBoxService.Show("Funcionário não possui senha. Configure pela primeira vez.", "Senha de Funcionário", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }

            ConfirmarComando = new RelayCommand(Confirmar, ConfirmarValidacao);
        }

        private async void Confirmar(object obj)
        {
            bool flag;
            if (PossuiSenhaAtual)
            {
                flag = Autenticacao.ConfirmaSenha(SenhaAtual, Funcionario.Senha);
            }
            else
            {
                flag = true;
            }

            if (flag)
            {
                Funcionario.Senha = Autenticacao.EncriptaEmMD5(NovaSenha);
                try
                {
                    await daoFuncionario.Atualizar(Funcionario);
                    messageBoxService.Show("Senha alterada com sucesso.");
                    RequestClose?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    messageBoxService.Show(ex.Message);
                }
            }
            else
            {
                messageBoxService.Show("Senha atual informada está incorreta");
            }
        }

        private bool ConfirmarValidacao(object arg)
        {
            if (PossuiSenhaAtual && (SenhaAtual == null || SenhaAtual.Length == 0))
            {
                return false;
            }

            if (NovaSenha == null || NovaSenha.Length == 0)
                return false;

            return true;
        }

        //Não possui o OnPropertyChanged porque atribuição ocorre no code-behind
        public string SenhaAtual
        {
            get
            {
                return _senhaAtual;
            }

            set
            {
                _senhaAtual = value;
            }
        }

        //Não possui o OnPropertyChanged porque atribuição ocorre no code-behind
        public string NovaSenha
        {
            get
            {
                return _novaSenha;
            }

            set
            {
                _novaSenha = value;
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

        public bool PossuiSenhaAtual
        {
            get
            {
                return _possuiSenhaAtual;
            }

            set
            {
                _possuiSenhaAtual = value;
                OnPropertyChanged("PossuiSenhaAtual");
            }
        }
    }
}

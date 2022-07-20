using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using VandaModaIntimaWpf.ViewModel.Interfaces;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using VandaModaIntimaWpf.ViewModel.Util;

namespace VandaModaIntimaWpf.ViewModel.PontoEletronico
{
    public class InserirSenhaPontoVM : IRequestClose
    {
        private string _senha;
        private IMessageBoxService messageBoxService;
        private Model.Funcionario funcionario;
        private bool _resultado = false;

        public event EventHandler<EventArgs> RequestClose;

        public ICommand ConfirmarSenhaComando { get; set; }

        public InserirSenhaPontoVM(Model.Funcionario funcionario)
        {
            ConfirmarSenhaComando = new RelayCommand(ConfirmarSenha, ConfirmarSenhaValidacao);
            this.funcionario = funcionario;
            messageBoxService = new MessageBoxService();
        }

        private void ConfirmarSenha(object obj)
        {
            Resultado = Autenticacao.ConfirmaSenha(Senha, funcionario.Senha);
            if (Resultado)
            {
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                messageBoxService.Show("Senha incorreta!", "Tela de Login", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
        private bool ConfirmarSenhaValidacao(object arg)
        {
            if (Senha?.Length == 0)
                return false;
            return true;
        }

        public string Senha
        {
            get
            {
                return _senha;
            }

            set
            {
                _senha = value;
            }
        }

        public bool Resultado
        {
            get
            {
                return _resultado;
            }

            set
            {
                _resultado = value;
            }
        }
    }
}

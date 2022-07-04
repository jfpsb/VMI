using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.PontoEletronico
{
    public class InserirSenhaPontoVM
    {
        private string _senha;
        private IMessageBoxService messageBoxService;

        public ICommand ConfirmarSenhaComando { get; set; }

        public InserirSenhaPontoVM(Model.Funcionario funcionario)
        {
            ConfirmarSenhaComando = new RelayCommand(ConfirmarSenha, ConfirmarSenhaValidacao);
            messageBoxService = new MessageBoxService();
        }

        private void ConfirmarSenha(object obj)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(Encoding.ASCII.GetBytes(Senha));
            var emString = Encoding.ASCII.GetString(md5.Hash);

            if (emString.Equals(Senha))
            {
                messageBoxService.Show("");
            }
            else
            {
                messageBoxService.Show("Senha incorreta!");
            }
        }
        private bool ConfirmarSenhaValidacao(object arg)
        {
            if (Senha.Length == 0)
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
    }
}

using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Windows.Input;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class CadastrarFornecedorOnlineViewModel : CadastrarFornecedorManualmenteViewModel
    {
        public ICommand PesquisarComando { get; set; }

        public CadastrarFornecedorOnlineViewModel()
        {
            PesquisarComando = new RelayCommand(Pesquisar, (object p) => { return Fornecedor.Cnpj?.Length == 14; });
        }

        public async void Pesquisar(object parameter)
        {
            var request = WebRequest.CreateHttp($"https://www.receitaws.com.br/v1/cnpj/{Fornecedor.Cnpj}");
            request.Method = "GET";
            //request.UserAgent = "VandaModaIntima";
            try
            {
                using (var response = await request.GetResponseAsync())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            object objResponse = streamReader.ReadToEnd();
                            Fornecedor = JsonConvert.DeserializeObject<FornecedorModel>(objResponse.ToString());

                            if (Fornecedor.Cnpj == null)
                            {
                                var jsonErro = JsonConvert.DeserializeObject<JsonErro>(objResponse.ToString());
                                SetStatusBarErroPesquisa(jsonErro.Message);
                            }
                            else
                            {
                                SetStatusBarSucessoPesquisa();
                            }
                        }
                    }
                }
            }
            catch (WebException we)
            {
                if (we.Message.Contains("429"))
                {
                    SetStatusBarErroPesquisa("Aguarde Um Pouco Entre Cada Consulta");
                }
                else
                {
                    SetStatusBarErroPesquisa(we.Message);
                }
            }
        }

        private void SetStatusBarErroPesquisa(string mensagem)
        {
            MensagemStatusBar = mensagem;
            ImagemStatusBar = IMAGEMERRO;
        }

        private void SetStatusBarSucessoPesquisa()
        {
            MensagemStatusBar = "Fornecedor Encontrado";
            ImagemStatusBar = IMAGEMSUCESSO;
        }
    }

    class JsonErro
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}

using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    public class RequisicaoReceitaFederal
    {
        public async Task<FornecedorModel> GetFornecedor(string cnpj)
        {
            var requisicao = WebRequest.CreateHttp($"https://www.receitaws.com.br/v1/cnpj/{cnpj}");
            requisicao.Method = "GET";

            using (var respostaWeb = await requisicao.GetResponseAsync())
            {
                using (var stream = respostaWeb.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(stream))
                    {
                        object resposta = streamReader.ReadToEnd();
                        FornecedorModel Fornecedor = JsonConvert.DeserializeObject<FornecedorModel>(resposta.ToString());

                        if (Fornecedor.Cnpj == null)
                        {
                            var jsonErro = JsonConvert.DeserializeObject<JsonErro>(resposta.ToString());
                            throw new InvalidDataException(jsonErro.Message);
                        }

                        return Fornecedor;
                    }
                }
            }
        }
        private class JsonErro
        {
            public string Status { get; set; }
            public string Message { get; set; }
        }
    }
}

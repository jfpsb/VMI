using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.BancoDeDados.CouchDb;

namespace VandaModaIntimaWpf.BancoDeDados
{
    public sealed class CouchDbClient
    {
        private static string CouchDbAddress = "http://{0}:{1}";
        private static string Server = "localhost";
        private static string Port = "5984";

        private static CookieContainer CookieContainer;
        private static HttpClientHandler ClientHandler;
        private static HttpClient httpClient;
        private static string AuthCouchDbCookieKeyName = "AuthSession";
        private static string AuthCookie = null;

        private static readonly Lazy<CouchDbClient> lazyClient = new Lazy<CouchDbClient>(() => new CouchDbClient());

        private CouchDbClient()
        {
            GetAuthenticationCookie();
        }

        public CouchDbResponse CreateDatabase(string database)
        {
            string url = string.Format("{0}/{1}", CouchDbAddress, database);
            return RunPUTRequest(url);
        }

        public async Task<CouchDbResponse> CreateDocument(CouchDbLog log)
        {
            GetAuthenticationCookie();

            var jsonData = JsonConvert.SerializeObject(log);

            CookieContainer.Add(new Uri(CouchDbAddress), new Cookie(AuthCouchDbCookieKeyName, AuthCookie));
            var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await httpClient.PutAsync(string.Format("/{0}/{1}", log.TipoEntidade, log.Id), httpContent);

            CouchDbResponse couchDbResponse;

            if (result.IsSuccessStatusCode)
            {
                couchDbResponse = JsonConvert.DeserializeObject<CouchDbResponse>(result.Content.ReadAsStringAsync().Result);
                Console.WriteLine(result.Content.ReadAsStringAsync().Result);
            }
            else
            {
                throw new Exception(string.Format("Erro Ao Criar Documento. Status Code: {0};\n\nMensagem: {1}", result.StatusCode.ToString(), result.Content.ReadAsStringAsync().Result));
            }

            return couchDbResponse;
        }

        public async Task<CouchDbResponse> UpdateDocument(CouchDbLog couchDbLog)
        {
            GetAuthenticationCookie();
            CookieContainer.Add(new Uri(CouchDbAddress), new Cookie(AuthCouchDbCookieKeyName, AuthCookie));
            string jsonData = JsonConvert.SerializeObject(couchDbLog);
            var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await httpClient.PutAsync(string.Format("/{0}/{1}", couchDbLog.TipoEntidade.ToLower(), couchDbLog.Id), httpContent);
            CouchDbResponse couchDbResponse;

            if (result.IsSuccessStatusCode)
            {
                couchDbResponse = JsonConvert.DeserializeObject<CouchDbResponse>(result.Content.ReadAsStringAsync().Result);
                Console.WriteLine(result.Content.ReadAsStringAsync().Result);
            }
            else
            {
                throw new Exception(string.Format("Erro Ao Atualizar Documento. Status Code: {0};\n\nMensagem: {1}", result.StatusCode.ToString(), result.Content.ReadAsStringAsync().Result));
            }

            return couchDbResponse;
        }
        public async Task<CouchDbResponse> DeleteDocument(CouchDbLog couchDbLog)
        {
            CouchDbResponse couchDbResponse;

            GetAuthenticationCookie();
            CookieContainer.Add(new Uri(CouchDbAddress), new Cookie(AuthCouchDbCookieKeyName, AuthCookie));
            HttpResponseMessage result = await httpClient.DeleteAsync(string.Format("{0}/{1}/{2}?rev={3}", CouchDbAddress, couchDbLog.TipoEntidade, couchDbLog.Id, couchDbLog.Rev));

            if (result.IsSuccessStatusCode)
            {
                couchDbResponse = JsonConvert.DeserializeObject<CouchDbResponse>(result.Content.ReadAsStringAsync().Result);
                Console.WriteLine(result.Content.ReadAsStringAsync().Result);
            }
            else
            {
                throw new Exception(string.Format("Erro Ao Deletar Documento. Status Code: {0};\n\nMensagem: {1}", result.StatusCode.ToString(), result.Content.ReadAsStringAsync().Result));
            }

            return couchDbResponse;
        }

        private CouchDbResponse RunPUTRequest(string url)
        {
            CouchDbResponse couchDbResponse = new CouchDbResponse();
            string tipoRequisicao = "PUT";
            string requisicaoUrl = url;

            var httpRequest = WebRequest.CreateHttp(requisicaoUrl);
            httpRequest.Method = tipoRequisicao;
            httpRequest.ContentType = "application/json";

            using (var httpResponse = (HttpWebResponse)httpRequest.GetResponse())
            {
                using (var stream = httpResponse.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var responseText = reader.ReadToEnd();
                        if (httpResponse.StatusCode == HttpStatusCode.OK)
                        {
                            couchDbResponse = JsonConvert.DeserializeObject<CouchDbResponse>(responseText);
                        }
                        else
                        {
                            Console.WriteLine(responseText);
                        }
                    }
                }

            }

            return couchDbResponse;
        }

        public async Task<CouchDbLog> FindById(string id, string database, bool revs_info = false)
        {
            CouchDbLog log = null;

            if (id != null)
            {
                GetAuthenticationCookie();
                CookieContainer.Add(new Uri(CouchDbAddress), new Cookie(AuthCouchDbCookieKeyName, AuthCookie));

                string requestUri = "/{0}/{1}";
                if (revs_info)
                {
                    requestUri += "?revs_info=true";
                }

                HttpResponseMessage result = await httpClient.GetAsync(string.Format(requestUri, database, id));

                if (result.IsSuccessStatusCode)
                {
                    string responseText = result.Content.ReadAsStringAsync().Result;
                    log = JsonConvert.DeserializeObject<CouchDbLog>(responseText);
                }
            }

            return log;
        }

        public void GetAuthenticationCookie()
        {
            if (AuthCookie == null)
            {
                if (httpClient == null)
                {
                    CouchDbAddress = string.Format(CouchDbAddress, Server, Port);
                    CookieContainer = new CookieContainer();
                    ClientHandler = new HttpClientHandler() { CookieContainer = CookieContainer };
                    httpClient = new HttpClient(ClientHandler) { BaseAddress = new Uri(CouchDbAddress) };
                }

                CouchDbCredentials credentials = new CouchDbCredentials()
                {
                    Username = "admin",
                    Password = "1124"
                };

                string authPayload = JsonConvert.SerializeObject(credentials);
                var authResult = httpClient.PostAsync("/_session", new StringContent(authPayload, Encoding.UTF8, "application/json")).Result;
                if (authResult.IsSuccessStatusCode)
                {
                    var responseHeaders = authResult.Headers.ToList();
                    string plainResponseLoad = authResult.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("Authenticated user from CouchDB API:");
                    Console.WriteLine(plainResponseLoad);
                    var authCookie = responseHeaders.Where(r => r.Key == "Set-Cookie").Select(r => r.Value.ElementAt(0)).FirstOrDefault();
                    if (authCookie != null)
                    {
                        int cookieValueStart = authCookie.IndexOf("=") + 1;
                        int cookieValueEnd = authCookie.IndexOf(";");
                        int cookieLength = cookieValueEnd - cookieValueStart;
                        string authCookieValue = authCookie.Substring(cookieValueStart, cookieLength);
                        AuthCookie = authCookieValue;
                    }
                    else
                    {
                        throw new Exception("There is no auth cookie header in the response from the CouchDB API");
                    }
                }
                else
                {
                    throw new HttpRequestException(string.Concat("Authentication failure: ", authResult.ReasonPhrase));
                }
            }
        }

        public static CouchDbClient Instancia
        {
            get
            {
                return lazyClient.Value;
            }
        }
    }
}

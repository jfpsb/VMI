using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.BancoDeDados.Model;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados
{
    public sealed class CouchDbClient
    {
        private static string CouchDbAddress = "http://{0}:{1}";
        private static string Server = "localhost";
        private static string Port = "5984";
        private static string Database = "vmilog";

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

        public async Task<CouchDbResponse> CreateDocument(string id, string jsonData)
        {
            CouchDbResponse couchDbResponse = new CouchDbResponse();
            GetAuthenticationCookie();
            CookieContainer.Add(new Uri(CouchDbAddress), new Cookie(AuthCouchDbCookieKeyName, AuthCookie));
            var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await httpClient.PutAsync(string.Format("/{0}/{1}", Database, id), httpContent);

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
        public async Task<CouchDbResponse> CreateDocument<E>(IList<E> collection)
        {
            CouchDbResponse couchDbResponse = new CouchDbResponse();
            GetAuthenticationCookie();
            CookieContainer.Add(new Uri(CouchDbAddress), new Cookie(AuthCouchDbCookieKeyName, AuthCookie));

            CouchDbBulkDocs couchDbBulkDocs = new CouchDbBulkDocs();

            foreach (E e in collection)
            {
                switch (e)
                {
                    case Adiantamento a:
                        CouchDbAdiantamentoLog aLog = new CouchDbAdiantamentoLog()
                        {
                            Id = a.Id.ToString()
                        };
                        aLog.AtribuiCampos(a);
                        couchDbBulkDocs.Docs.Add(aLog);
                        break;
                    case Bonus b:
                        CouchDbBonusLog bLog = new CouchDbBonusLog()
                        {
                            Id = b.Id.ToString()
                        };
                        bLog.AtribuiCampos(b);
                        couchDbBulkDocs.Docs.Add(bLog);
                        break;
                    case Contagem c:
                        CouchDbContagemLog CLog = new CouchDbContagemLog()
                        {
                            Id = c.CouchDbId()
                        };
                        CLog.AtribuiCampos(c);
                        couchDbBulkDocs.Docs.Add(CLog);
                        break;
                    case ContagemProduto cp:
                        CouchDbContagemProdutoLog cpLog = new CouchDbContagemProdutoLog()
                        {
                            Id = cp.Id.ToString()
                        };
                        cpLog.AtribuiCampos(cp);
                        couchDbBulkDocs.Docs.Add(cpLog);
                        break;
                    case FolhaPagamento fp:
                        CouchDbFolhaPagamentoLog fpLog = new CouchDbFolhaPagamentoLog()
                        {
                            Id = fp.Id
                        };
                        fpLog.AtribuiCampos(fp);
                        couchDbBulkDocs.Docs.Add(fpLog);
                        break;
                    case Fornecedor forn:
                        CouchDbFornecedorLog fornLog = new CouchDbFornecedorLog()
                        {
                            Id = forn.Cnpj
                        };
                        fornLog.AtribuiCampos(forn);
                        couchDbBulkDocs.Docs.Add(fornLog);
                        break;
                    case Funcionario func:
                        CouchDbFuncionarioLog funcLog = new CouchDbFuncionarioLog()
                        {
                            Id = func.Cpf
                        };
                        funcLog.AtribuiCampos(func);
                        couchDbBulkDocs.Docs.Add(funcLog);
                        break;
                    case Loja l:
                        CouchDbLojaLog lLog = new CouchDbLojaLog()
                        {
                            Id = l.Cnpj
                        };
                        lLog.AtribuiCampos(l);
                        couchDbBulkDocs.Docs.Add(lLog);
                        break;
                    case Marca m:
                        CouchDbMarcaLog mLog = new CouchDbMarcaLog()
                        {
                            Id = m.Nome
                        };
                        mLog.AtribuiCampos(m);
                        couchDbBulkDocs.Docs.Add(mLog);
                        break;
                    case MetaLoja ml:
                        CouchDbMetaLojaLog mlLog = new CouchDbMetaLojaLog()
                        {
                            Id = ml.Id
                        };
                        mlLog.AtribuiCampos(ml);
                        couchDbBulkDocs.Docs.Add(mlLog);
                        break;
                    case OperadoraCartao oc:
                        CouchDbOperadoraCartaoLog ocLog = new CouchDbOperadoraCartaoLog()
                        {
                            Id = oc.Nome
                        };
                        ocLog.AtribuiCampos(oc);
                        couchDbBulkDocs.Docs.Add(ocLog);
                        break;
                    case Parcela p:
                        CouchDbParcelaLog pLog = new CouchDbParcelaLog()
                        {
                            Id = p.Id.ToString()
                        };
                        pLog.AtribuiCampos(p);
                        couchDbBulkDocs.Docs.Add(pLog);
                        break;
                    case Produto pt:
                        CouchDbProdutoLog ptLog = new CouchDbProdutoLog()
                        {
                            Id = pt.CodBarra
                        };
                        ptLog.AtribuiCampos(pt);
                        couchDbBulkDocs.Docs.Add(ptLog);
                        break;
                    case RecebimentoCartao rc:
                        CouchDbRecebimentoCartaoLog rcLog = new CouchDbRecebimentoCartaoLog()
                        {
                            Id = rc.CouchDbId()
                        };
                        rcLog.AtribuiCampos(rc);
                        couchDbBulkDocs.Docs.Add(rcLog);
                        break;
                    case TipoContagem tc:
                        CouchDbMetaLojaLog tcLog = new CouchDbMetaLojaLog()
                        {
                            Id = tc.Id.ToString()
                        };
                        tcLog.AtribuiCampos(tc);
                        couchDbBulkDocs.Docs.Add(tcLog);
                        break;
                }
            }

            string jsonData = JsonConvert.SerializeObject(couchDbBulkDocs);
            var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await httpClient.PostAsync(string.Format("/{0}/{1}", Database, "_bulk_docs"), httpContent);

            if (result.IsSuccessStatusCode)
            {
                couchDbResponse = JsonConvert.DeserializeObject<CouchDbResponse>(result.Content.ReadAsStringAsync().Result);
                Console.WriteLine(result.Content.ReadAsStringAsync().Result);
            }
            else
            {
                throw new Exception(string.Format("Erro Ao Criar Documentos. Status Code: {0};\n\nMensagem: {1}", result.StatusCode.ToString(), result.Content.ReadAsStringAsync().Result));
            }

            return couchDbResponse;
        }

        public async Task<CouchDbResponse> UpdateDocument(CouchDbLog couchDbLog)
        {
            CouchDbResponse couchDbResponse = new CouchDbResponse();
            GetAuthenticationCookie();
            CookieContainer.Add(new Uri(CouchDbAddress), new Cookie(AuthCouchDbCookieKeyName, AuthCookie));
            string jsonData = JsonConvert.SerializeObject(couchDbLog);
            var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await httpClient.PutAsync(string.Format("/{0}/{1}", Database, couchDbLog.Id), httpContent);

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
        public async Task<CouchDbResponse> DeleteDocument(string id, string rev)
        {
            CouchDbResponse couchDbResponse = new CouchDbResponse();
            GetAuthenticationCookie();
            CookieContainer.Add(new Uri(CouchDbAddress), new Cookie(AuthCouchDbCookieKeyName, AuthCookie));
            HttpResponseMessage result = await httpClient.DeleteAsync(string.Format("{0}/{1}/{2}?rev={3}", CouchDbAddress, Database, id, rev));

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
        public async Task<CouchDbLog> FindById(string id, bool revs_info = false)
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

                HttpResponseMessage result = await httpClient.GetAsync(string.Format(requestUri, Database, id));

                if (result.IsSuccessStatusCode)
                {
                    string responseText = result.Content.ReadAsStringAsync().Result;
                    log = JsonConvert.DeserializeObject<CouchDbLog>(responseText);

                    switch (log.Tipo)
                    {
                        case "adiantamento":
                            log = JsonConvert.DeserializeObject<CouchDbAdiantamentoLog>(responseText);
                            break;
                        case "bonus":
                            log = JsonConvert.DeserializeObject<CouchDbBonusLog>(responseText);
                            break;
                        case "contagem":
                            log = JsonConvert.DeserializeObject<CouchDbContagemLog>(responseText);
                            break;
                        case "contagemproduto":
                            log = JsonConvert.DeserializeObject<CouchDbContagemProdutoLog>(responseText);
                            break;
                        case "folhapagamento":
                            log = JsonConvert.DeserializeObject<CouchDbFolhaPagamentoLog>(responseText);
                            break;
                        case "fornecedor":
                            log = JsonConvert.DeserializeObject<CouchDbFornecedorLog>(responseText);
                            break;
                        case "funcionario":
                            log = JsonConvert.DeserializeObject<CouchDbFuncionarioLog>(responseText);
                            break;
                        case "loja":
                            log = JsonConvert.DeserializeObject<CouchDbLojaLog>(responseText);
                            break;
                        case "marca":
                            log = JsonConvert.DeserializeObject<CouchDbMarcaLog>(responseText);
                            break;
                        case "metaloja":
                            log = JsonConvert.DeserializeObject<CouchDbMetaLojaLog>(responseText);
                            break;
                        case "operadoracartao":
                            log = JsonConvert.DeserializeObject<CouchDbOperadoraCartaoLog>(responseText);
                            break;
                        case "parcela":
                            log = JsonConvert.DeserializeObject<CouchDbParcelaLog>(responseText);
                            break;
                        case "produto":
                            log = JsonConvert.DeserializeObject<CouchDbProdutoLog>(responseText);
                            break;
                        case "recebimentocartao":
                            log = JsonConvert.DeserializeObject<CouchDbRecebimentoCartaoLog>(responseText);
                            break;
                        case "tipocontagem":
                            log = JsonConvert.DeserializeObject<CouchDbTipoContagemLog>(responseText);
                            break;
                    }
                }
            }

            return log;
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

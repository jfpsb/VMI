using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using VandaModaIntimaWpf.BancoDeDados.Model;

namespace VandaModaIntimaWpf.BancoDeDados
{
    public class CouchDbClient
    {
        private static string Url = "http://{0}:{1}/{2}";
        private static string Server = "localhost";
        private static string Port = "5984";
        private static string Database = "vmi_log";

        public CouchDbClient()
        {
            Url = string.Format(Url, Server, Port);
        }

        public CouchDbResponse CreateDatabase(string database)
        {
            string url = string.Format("{0}/{1}", Url, database);
            return RunPUTRequest(url);
        }

        public CouchDbResponse CreateOrUpdateDocument(string id, string jsonData)
        {
            string url = string.Format("{0}/{1}/{2} -d \"{3}\"", Url, Database, id, jsonData);
            return RunPUTRequest(url);
        }
        public CouchDbResponse DeleteDocument(string id)
        {
            CouchDbResponse couchDbResponse = new CouchDbResponse();
            string tipoRequisicao = "DELETE";
            string requisicaoUrl = string.Format("{0}/{1}/{2}", Url, Database, id);

            var httpRequest = WebRequest.CreateHttp(requisicaoUrl);
            httpRequest.Method = tipoRequisicao;

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
        public CouchDbLog<E> FindById<E>(string id, bool revs_info = false)
        {
            CouchDbLog<E> log = null;
            string tipoRequisicao = "GET";
            string requisicaoUrl = string.Format("{0}/{1}/{2}", Url, Database, id);

            if (revs_info)
                requisicaoUrl = string.Format("{0}?revs_info=true", requisicaoUrl);

            var httpRequest = WebRequest.CreateHttp(requisicaoUrl);
            httpRequest.Method = tipoRequisicao;

            using (var httpResponse = (HttpWebResponse)httpRequest.GetResponse())
            {
                using (var stream = httpResponse.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var responseText = reader.ReadToEnd();

                        if (httpResponse.StatusCode == HttpStatusCode.OK)
                        {
                            log = JsonConvert.DeserializeObject<CouchDbLog<E>>(responseText);
                        }
                        else
                        {
                            Console.WriteLine(responseText);
                        }
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
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHibernate;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf
{
    public sealed class Config
    {
        private static readonly Lazy<Config> lazyClient = new Lazy<Config>(() => new Config());

        public static readonly string AppDocumentsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Vanda Moda Íntima");
        public static Config Instancia => lazyClient.Value;

        public static Model.Loja LojaAplicacao(ISession session)
        {
            var configJson = File.ReadAllText(Path.Combine(AppDocumentsFolder, "Config.json"));
            JObject json = JObject.Parse(configJson);
            var cnpj = json["loja"]["cnpj"].ToString();
            return session.Get<Model.Loja>(cnpj);
        }

        private static JObject GetCredenciaisEncriptadas(Model.Loja loja)
        {
            try
            {
                var configJson = File.ReadAllText(Path.Combine(AppDocumentsFolder, "Config.json"));
                JObject json = JObject.Parse(configJson);
                string credencialId = loja.Cnpj.Substring(0, 8);
                return JObject.Parse(json["credenciais_pix"][credencialId].ToString());
            }
            catch (Exception ex)
            {
                Log.EscreveLogCredenciais(ex);
                return null;
            }
        }

        public static JObject GNEndpoints(ISession session)
        {
            try
            {
                JObject credentials_encrypted = GetCredenciaisEncriptadas(LojaAplicacao(session));

                byte[] clientIDEncrypted = Convert.FromBase64String((string)credentials_encrypted["client_id"]);
                byte[] clientSecretEncrypted = Convert.FromBase64String((string)credentials_encrypted["client_secret"]);

                var credentials = new
                {
                    client_id = Encoding.Unicode.GetString(ProtectedData.Unprotect(clientIDEncrypted, null, DataProtectionScope.LocalMachine)),
                    client_secret = Encoding.Unicode.GetString(ProtectedData.Unprotect(clientSecretEncrypted, null, DataProtectionScope.LocalMachine)),
                    pix_cert = (string)credentials_encrypted["pix_cert"],
                    sandbox = (string)credentials_encrypted["sandbox"]
                };

                return JObject.Parse(JsonConvert.SerializeObject(credentials));
            }
            catch (Exception ex)
            {
                Log.EscreveLogCredenciais(ex);
                return null;
            }
        }

        public static JObject GetDadosRecebedor(Model.Loja loja)
        {
            try
            {
                var configJson = File.ReadAllText(Path.Combine(AppDocumentsFolder, "Config.json"));
                JObject json = JObject.Parse(configJson);
                string lojaId = loja.Cnpj.Substring(0, 8);
                return JObject.Parse(json["dados_recebedor"][lojaId].ToString());
            }
            catch (Exception ex)
            {
                Log.EscreveLogCredenciais(ex);
                return null;
            }
        }

        public double ValorDiarioPassagemOnibus
        {
            get
            {
                var configJson = File.ReadAllText(Path.Combine(AppDocumentsFolder, "Config.json"));
                var json = JObject.Parse(configJson);
                var valor = (double)json["valor_diario_passagem_onibus"];
                return valor;
            }
            set
            {
                var configJson = File.ReadAllText(Path.Combine(AppDocumentsFolder, "Config.json"));
                var json = JObject.Parse(configJson);
                json["valor_diario_passagem_onibus"] = value;
                File.WriteAllText(Path.Combine(AppDocumentsFolder, "Config.json"), json.ToString());
            }
        }
        public double ValorDiarioValeAlimentacao
        {
            get
            {
                var configJson = File.ReadAllText(Path.Combine(AppDocumentsFolder, "Config.json"));
                var json = JObject.Parse(configJson);
                var valor = (double)json["valor_diario_vale_alimentacao"];
                return valor;
            }
            set
            {
                var configJson = File.ReadAllText(Path.Combine(AppDocumentsFolder, "Config.json"));
                var json = JObject.Parse(configJson);
                json["valor_diario_vale_alimentacao"] = value;
                File.WriteAllText(Path.Combine(AppDocumentsFolder, "Config.json"), json.ToString());
            }
        }
    }
}

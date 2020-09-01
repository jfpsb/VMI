using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using VandaModaIntimaWpf.BancoDeDados;

namespace VandaModaIntimaWpf.ViewModel
{
    public sealed class GlobalConfigs
    {
        private static readonly string CONFIGS_FILE_PATH = @"Config.xml";
        private static readonly string LOGSAENVIAR_FILE_PATH = @"LogsAEnviar.json";
        private static XmlDocument configsXml;
        private static readonly Lazy<GlobalConfigs> lazyConfigs = new Lazy<GlobalConfigs>(() => new GlobalConfigs());

        public string CLIENT_ID;
        private static Queue<string> LOGS_A_ENVIAR;

        public static GlobalConfigs Instancia => lazyConfigs.Value;

        private GlobalConfigs()
        {
            CarregaConfigsXml();
            CarregaLogsAEnviar();
        }

        private void CarregaConfigsXml()
        {
            configsXml = new XmlDocument();

            if (File.Exists(CONFIGS_FILE_PATH))
            {
                configsXml.Load(CONFIGS_FILE_PATH);
            }
            else
            {
                //Se não existir arquivo de configurações, ele é criado
                XmlElement clienteIdElement = configsXml.CreateElement("ClienteId");
                clienteIdElement.InnerText = Guid.NewGuid().ToString();
                configsXml.AppendChild(clienteIdElement);
                configsXml.Save(CONFIGS_FILE_PATH);
            }

            XmlNode clientIdNode = configsXml.SelectSingleNode("ClienteId");

            CLIENT_ID = clientIdNode.InnerText;
        }

        private static void CarregaLogsAEnviar()
        {
            if (File.Exists(LOGSAENVIAR_FILE_PATH))
            {
                string logsAEnviarJson = File.ReadAllText(LOGSAENVIAR_FILE_PATH);
                LOGS_A_ENVIAR = JsonConvert.DeserializeObject<Queue<string>>(logsAEnviarJson);
            }
            else
            {
                LOGS_A_ENVIAR = new Queue<string>();
            }
        }

        public void AddLogAEnviar(string id)
        {
            LOGS_A_ENVIAR.Enqueue(id);
        }

        public void SalvarLogsAEnviarEmJson()
        {
            File.WriteAllText(LOGSAENVIAR_FILE_PATH, JsonConvert.SerializeObject(LOGS_A_ENVIAR));
        }

        public void EnviarLogsMqtt()
        {
            while (LOGS_A_ENVIAR.Count > 0)
            {
                string id = LOGS_A_ENVIAR.Peek();
                var log = CouchDbClient.Instancia.FindById(id);
                string logJson = JsonConvert.SerializeObject(log);
                MqttClientWrapper.Instancia.Client.Publish($"{CLIENT_ID}/vandamodaintima/{log.Result.Tipo}/{log.Result.Id}", Encoding.UTF8.GetBytes(logJson));
            }

            SalvarLogsAEnviarEmJson();
        }
    }
}

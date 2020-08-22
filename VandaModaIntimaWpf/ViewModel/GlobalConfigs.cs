using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using VandaModaIntimaWpf.BancoDeDados;

namespace VandaModaIntimaWpf.ViewModel
{
    public class GlobalConfigs
    {
        private static readonly string CONFIGS_FILE_PATH = @"Config.xml";
        private static readonly string DATABASELOG_FILE_PATH = @"DatabaseLog.json";
        private static XmlDocument configsXml;

        public static string CLIENT_ID;
        public static List<int> DATABASELOG;
        public GlobalConfigs()
        {
            CarregaConfigsXml();
            CarregaDatabaseLog();
        }

        private static void CarregaConfigsXml()
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

        private static void CarregaDatabaseLog()
        {
            //DATABASELOG = new List<DatabaseLog>();

            //if (File.Exists(DATABASELOG_FILE_PATH))
            //{
            //    string DatabaseLogJson = File.ReadAllText(DATABASELOG_FILE_PATH);
            //    DATABASELOG = JsonConvert.DeserializeObject<List<DatabaseLog>>(DatabaseLogJson);
            //}
        }

        public static void SalvaDatabaseLog()
        {
            string json = JsonConvert.SerializeObject(DATABASELOG, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(DATABASELOG_FILE_PATH, json);
        }
    }
}

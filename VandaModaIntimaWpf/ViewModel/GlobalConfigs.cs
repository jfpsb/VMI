using System.Xml;

namespace VandaModaIntimaWpf.ViewModel
{
    public class GlobalConfigs
    {
        private static XmlDocument configsXml;
        public GlobalConfigs()
        {
            configsXml = new XmlDocument();
            configsXml.Load(@"Config.xml");
        }

        public static string ClientId()
        {
            XmlNode node = configsXml.SelectSingleNode("ClienteId");
            return node.InnerText;
        }
    }
}

using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace VandaModaIntimaWpf.ViewModel
{
    public sealed class MqttClientWrapper
    {
        public MqttClient Client;
        private static readonly Lazy<MqttClientWrapper> LazyMqttClient = new Lazy<MqttClientWrapper>(() => new MqttClientWrapper());
        private MqttClientWrapper()
        {
            Client = new MqttClient("localhost");
            Client.Subscribe(new string[] { "+/vandamodaintima/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            Client.Connect(GlobalConfigs.Instancia.CLIENT_ID, null, null, false, 3);
        }

        public static MqttClientWrapper Instancia => LazyMqttClient.Value;
    }
}

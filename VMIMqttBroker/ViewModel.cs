using uPLibrary.Networking.M2Mqtt;

namespace VMIMqttBroker
{
    public class ViewModel : ICloseable
    {
        MqttBroker broker;

        public ViewModel()
        {
            broker = new MqttBroker();
            broker.Start();
        }

        public void Close()
        {
            broker.Stop();
        }
    }
}

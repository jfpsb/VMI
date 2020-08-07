using uPLibrary.Networking.M2Mqtt;

namespace VMIMqttBroker
{
    public class ViewModel : ObservableObjectBroker, ICloseable
    {
        private MqttBroker broker;
        private string _brokerVerbose;

        public ViewModel()
        {
            broker = new MqttBroker();
            broker.ClientConnected += Broker_ClientConnected;
            broker.ClientDisconnected += Broker_ClientDisconnected;
            broker.Start();
        }

        private void Broker_ClientDisconnected(MqttClient obj)
        {
            BrokerVerbose += string.Format("Cliente Desconectou com ID: {0}\n", obj.ClientId);
        }

        private void Broker_ClientConnected(MqttClient obj)
        {
            BrokerVerbose += string.Format("Cliente Conectou com ID: {0}\n", obj.ClientId);
        }

        public void Close()
        {
            broker.Stop();
        }

        public string BrokerVerbose
        {
            get => _brokerVerbose;
            set
            {
                _brokerVerbose = value;
                OnPropertyChanged("BrokerVerbose");
            }
        }
    }
}

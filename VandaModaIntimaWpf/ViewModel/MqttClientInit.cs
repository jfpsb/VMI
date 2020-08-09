using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace VandaModaIntimaWpf.ViewModel
{
    public class MqttClientInit
    {
        public static MqttClient MqttCliente;
        public MqttClientInit()
        {
            MqttCliente = new MqttClient("localhost");

            MqttCliente.Subscribe(new string[] { "+/vandamodaintima/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            // Retirando auto inscrição
            var v = MqttCliente.Unsubscribe(new string[] { string.Format("{0}/#", GlobalConfigs.CLIENT_ID) });

            MqttCliente.MqttMsgPublishReceived += MqttMsgPublishReceived;
            MqttCliente.MqttMsgPublished += MqttMsgPublished;
            MqttCliente.MqttMsgUnsubscribed += Unsubscribe;

            MqttCliente.Connect(GlobalConfigs.CLIENT_ID, null, null, false, 3);
        }

        private void Unsubscribe(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            
        }

        /// <summary>
        /// Executa quando uma mensagem é publicada ao broker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Console.WriteLine("ENVIADO");
        }

        /// <summary>
        /// Executa quando uma mensagem é recebida do broker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if (!e.Topic.Contains(GlobalConfigs.CLIENT_ID))
            {
                if (e.Topic.Contains("/adiantamento/"))
                {
                    string adiantamentoJson = Encoding.UTF8.GetString(e.Message);
                }
                else if (e.Topic.Contains("/bonus/"))
                {

                }
                else if (e.Topic.Contains("/contagem/"))
                {

                }
                else if (e.Topic.Contains("/contagemproduto/"))
                {

                }
                else if (e.Topic.Contains("/folhapagamento/"))
                {

                }
                else if (e.Topic.Contains("/fornecedor/"))
                {

                }
                else if (e.Topic.Contains("/funcionario/"))
                {

                }
                else if (e.Topic.Contains("/loja/"))
                {

                }
                else if (e.Topic.Contains("/marca/"))
                {

                }
                else if (e.Topic.Contains("/metaloja/"))
                {

                }
                else if (e.Topic.Contains("/operadoracartao/"))
                {

                }
                else if (e.Topic.Contains("/parcela/"))
                {

                }
                else if (e.Topic.Contains("/produto/"))
                {

                }
                else if (e.Topic.Contains("/recebimentocartao/"))
                {

                }
                else if (e.Topic.Contains("/tipocontagem/"))
                {

                }
            }
        }
    }
}

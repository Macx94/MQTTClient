using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;

namespace ClientMqtt_Test
{
    public class ClientManager
    {
        /// <summary>
        /// Connect the Client to the Broker
        /// </summary>
        /// <param name="Server">Address of the broker</param>
        /// <param name="MqttClient">Client istante to connect</param>
        /// <returns></returns>
        public async Task Connect(string Server, IMqttClient MqttClient)
        {
            try
            {
                var mqttOpzioni = new MqttClientOptionsBuilder()
                    .WithTcpServer(Server)
                    .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V311)
                    .Build();

                var response = await MqttClient.ConnectAsync(mqttOpzioni, CancellationToken.None);

                if (response.ResultCode == MqttClientConnectResultCode.Success)
                {
                    Console.WriteLine("Test di connessione completato con successo!");
                }
                else
                    Console.WriteLine("Attenzione: impossibile connettersi al Brocker!");
            }
            catch (Exception)
            {
                Console.WriteLine("Attenzione: impossibile connettersi al Brocker!");
            }
        }

        /// <summary>
        /// Disconnect the Client from the Brocker
        /// </summary>
        /// <param name="MqttFactory"></param>
        /// <param name="MqttClient"></param>
        /// <returns></returns>
        public async Task Disconnect(MqttFactory MqttFactory, IMqttClient MqttClient)
        {
            var mqttOpzioni = MqttFactory.CreateClientDisconnectOptionsBuilder().Build();
            await MqttClient.DisconnectAsync(mqttOpzioni, CancellationToken.None);
        }

        /// <summary>
        /// Exec the Publication to Topic
        /// </summary>
        /// <param name="Topic"></param>
        /// <param name="MqttClient"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public async Task Publish(string Topic, IMqttClient MqttClient, string Type, bool Retain = false, string Testo = "")
        {
            var msgCostruttore = new MqttApplicationMessageBuilder()
                .WithRetainFlag(Retain)
                .WithTopic(Topic);

            if (Type == "json")
            {
                Dictionary<string, string> objectJson = new Dictionary<string, string>();
                for (int i = 1; i < 5; i++)
                {
                    objectJson.Add(
                        String.Format("Sensore{0}", i),
                        GetRandomeNumber(i).ToString()
                        );
                }

                var payload = JsonConvert.SerializeObject(objectJson);
                msgCostruttore.WithPayload(payload);
            }
            else
            {
                if (String.IsNullOrEmpty(Testo))
                {
                    var valore = DateTime.Now.Millisecond;
                    msgCostruttore.WithPayload(valore.ToString());
                }
                else msgCostruttore.WithPayload(Testo);
            }

            var messaggio = msgCostruttore.Build();
            var result = MqttClient.PublishAsync(messaggio).Result;

            if (result.IsSuccess)
            {
                Console.WriteLine("Pubblicazione avvenuta con successo!");
            }
            else
            {
                Console.WriteLine("Errore in fase di pubblicazione!");
            }
        }

        private double GetRandomeNumber(int Seed)
        {
            Random rand = new Random(Seed);
            return rand.NextDouble();
        }
    }
}
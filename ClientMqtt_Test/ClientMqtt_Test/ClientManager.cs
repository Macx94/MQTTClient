using MQTTnet;
using MQTTnet.Client;

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
}
﻿using MQTTnet.Client;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMqtt_Test
{
    public class Subscription
    {
        /// <summary>
        /// Costruttore della classe, aggancia l'evento per l'ascolto dei messaggi
        /// </summary>
        /// <param name="Client"></param>
        public Subscription(IMqttClient Client)
        {
            Client.ApplicationMessageReceivedAsync += OnApplicationMessageReceivedAsync;
        }

        /// <summary>
        /// Connette il client al Broker
        /// </summary>
        /// <param name="Server"></param>
        /// <param name="MqttClient"></param
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
        /// Disconnette il client dal Broker
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
        /// Sottoscrive il Client al Topic specificato
        /// </summary>
        /// <param name="Topic"></param>
        /// <param name="MqttClient"></param>
        /// <returns></returns>
        public async Task Subscribe(string Topic, IMqttClient MqttClient)
        {
            try
            {
                MqttFactory mqttFactory = new MqttFactory();

                MqttClientSubscribeOptions options = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(
                        f => { f.WithTopic(Topic); }
                    )
                    .Build();

                MqttClientSubscribeResult result = await MqttClient.SubscribeAsync(options, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore in fase di avvio della sottoscrizione: {ex.Message}");
            }
        }

        /// <summary>
        /// Annulla la sottoscrizione del Client al Topic
        /// </summary>
        /// <param name="Topic"></param>
        /// <param name="MqttClient"></param>
        /// <returns></returns>
        public async Task Unsubscribe(string Topic, IMqttClient MqttClient)
        {
            try
            {
                await MqttClient.UnsubscribeAsync(Topic);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore in fase di rimozione della sottoscrizione: {ex.Message}");
            }
        }

        /// <summary>
        /// Evento che rimane in ascolto di eventuali messaggi ricevuti
        /// </summary>
        /// <param name="E"></param>
        /// <returns></returns>
        private async Task OnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs E)
        {
            try
            {
                string topic = E.ApplicationMessage.Topic;
                string payload = Encoding.UTF8.GetString(E.ApplicationMessage.Payload).TrimEnd('\0');

                Console.WriteLine($"Lettura avvenuta, il Topic {topic} ha un payload {payload}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore in fase di ricezione del messaggio: {ex.Message}");
            }
        }
    }
}

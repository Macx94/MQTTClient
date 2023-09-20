using ClientMqtt_Test;
using MQTTnet;
using MQTTnet.Client;

///Tengo la console sveglia finchè non sfrutto il comando per uscire
///non sono limitato ad un numero di comandi
///esco quando la condizione nel while non è più vera
bool _keepAlive = true;
string _command = "";

///Indirizzi di Broker Mqtt per test
///il 127.0.0.1 fa riferimento a quello di default di Eclipse-Mosquitto su Docker
string _server = "127.0.0.1";
//string _server = "test.mosquitto.org";
//string _server = "broker.hivemq.com";

string _subscribeTopic = "BIG/Test/2023";
string _publishTopic = "BIG/Test/2023";

ClientManager _managerMqtt = new ClientManager();
MqttFactory mqttFactory = new MqttFactory();

//Primo comando descrittivo
Console.Write($"Ciao,\n " +
    $"scrivi 'help' per avere un suggerimento.\n " +
    $"Scrivi 'exit' per uscire.\n");
do
{

    //Leggo l'input digitato nella console
    _command = Console.ReadLine();

    switch (_command.ToLower())
    {
        default:
            Console.Write("Comando non riconosciuto.\ninserisci un comando.\n");
            break;
        case ("exit"):
            //Esco senza stampare comandi
            _keepAlive = false;
            break;
        case ("help"):
            Console.WriteLine("publish: esegue una publicazione sul broker.");
            Console.WriteLine("subscribe: esegue una sottoscrizione sul broker per uno specifico Topic.");
            Console.WriteLine("timer: esegue un test dello Sleep.");
            Console.WriteLine("verifyconnection: esegue un test di connessione del client al broker.");
            break;
        case ("publish"):
            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                await _managerMqtt.Connect(_server, mqttClient);
                await _managerMqtt.Publish(_publishTopic, mqttClient);
                await _managerMqtt.Disconnect(mqttFactory, mqttClient);
            }
            break;
        case ("subscribe"):
            using (var mqttClient = (MqttClient)mqttFactory.CreateMqttClient())
            {
                Subscription sottoscrittore = new Subscription(mqttClient);
                await sottoscrittore.Connect(_server, mqttClient);
                await sottoscrittore.Subscribe(_subscribeTopic, mqttClient);
                Console.WriteLine($"Azione avviata con il Topic {_subscribeTopic}, durata dell'operazione 2 minuti ");
                Thread.Sleep(60000);
                Console.WriteLine("1 minuto ");
                Thread.Sleep(30000);
                Console.WriteLine("30 secondi....");
                Thread.Sleep(20000);
                Console.WriteLine("10 secondi....");
                Thread.Sleep(10000);
                Console.WriteLine("FINE!!!");
                await sottoscrittore.Unsubscribe(_subscribeTopic, mqttClient);
                await sottoscrittore.Disconnect(mqttFactory, mqttClient);
            }
            break;
        case ("timer"):
            Thread.Sleep(1000);
            Console.WriteLine("1 secondo");
            Thread.Sleep(1000);
            Console.WriteLine("2 secondi");
            Thread.Sleep(3000);
            Console.WriteLine("5 secondi");
            Thread.Sleep(5000);
            Console.WriteLine("10 secondi");
            Console.WriteLine("FINE");
            break;
        case ("verifyconnection"):
            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                await _managerMqtt.Connect(_server, mqttClient);
                await _managerMqtt.Disconnect(mqttFactory, mqttClient);
            }
            break;
    }
} while (_keepAlive);
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


ClientManager _managerMqtt = new ClientManager();

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
            Console.WriteLine("timer: esegue un test dello Sleep.");
            Console.WriteLine("verifyconnection: esegue un test di connessione del client al broker.");
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
            MqttFactory mqttFactory = new MqttFactory();
            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                await _managerMqtt.Connect(_server, mqttClient);
                await _managerMqtt.Disconnect(mqttFactory, mqttClient);
            }
            break;
    }
} while (_keepAlive);
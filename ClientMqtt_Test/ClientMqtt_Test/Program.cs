

bool _keepAlive = true;
string _command = "";

///Tengo la console sveglia finchè non sfrutto il comando per uscire
///non sono limitato ad un numero di comandi
///esco quando la condizione nel while non è più vera

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
    }
} while (_keepAlive);


bool _keepAlive = true;
string _command = "";

///Tengo la console sveglia finchè non sfrutto il comando per uscire
///non sono limitato ad un numero di comandi
///esco quando la condizione nel while non è più vera
do
{
    //Primo comando descrittivo
    Console.Write($"Ciao,\n" +
        $"Digita un comando oppure scrivi 'exit' per uscire.\n");

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
    }
} while (_keepAlive);
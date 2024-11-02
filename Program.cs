using System;
using System.Collections.Generic;
using System.IO;

// Classe principale du distributeur de tickets
class DistributeurTickets
{
    private static Dictionary<string, int> numerosTickets = new Dictionary<string, int>()
    {
        { "V", 0 },  // V pour Versement
        { "R", 0 },  // R pour Retrait
        { "I", 0 }   // I pour Informations
    };

    private static List<string> listeClients = new List<string>();
    private static string fichierNumeros = Path.Combine(Path.GetTempPath(), "fnumero.txt");
    private static string fichierClients = Path.Combine(Path.GetTempPath(), "clients.txt");

    static void Main()
    {
        ChargerNumeros();
        ChargerListeClients();

        while (true)
        {
            Console.WriteLine("Bienvenue au distributeur automatique de tickets.");
            Console.WriteLine("Quel type d'opération souhaitez-vous effectuer ?");
            Console.WriteLine("1 - Versement");
            Console.WriteLine("2 - Retrait");
            Console.WriteLine("3 - Informations");
            Console.WriteLine("4 - Quitter");
            Console.Write("Veuillez entrer le numéro correspondant à l'opération : ");

            string choix = Console.ReadLine();
            string typeOperation = "";

            switch (choix)
            {
                case "1":
                    typeOperation = "V";  // Versement
                    break;
                case "2":
                    typeOperation = "R";  // Retrait
                    break;
                case "3":
                    typeOperation = "I";  // Informations
                    break;
                case "4":
                    SauvegarderNumeros();
                    SauvegarderListeClients();
                    AfficherListeClients();
                    Console.WriteLine("Merci d'avoir utilisé le distributeur de tickets. Au revoir !");
                    return;
                default:
                    Console.WriteLine("Merci de choisir un nombre compris entre 1 et 3 pour effectuer une opération, tapez 4 pour quitter.");
                    continue;
            }

            string prenom = DemanderPrenom();
            string nom = DemanderNom();
            string numeroCarte = DemanderNumeroCarte();

            int numeroAttribue = AttribuerNumero(typeOperation);
            int personnesEnAttente = numeroAttribue - 1;

            Console.WriteLine($"\nVotre numéro est : {typeOperation}-{numeroAttribue}");
            if (personnesEnAttente == 0)
            {
                Console.WriteLine("Vous êtes le premier sur la liste.\n");
            }
            else
            {
                Console.WriteLine($"Il y a {personnesEnAttente} personne(s) qui attend(ent) avant vous.\n");
            }

            string clientInfo = $"Compte : {numeroCarte}, Nom : {nom}, Prénom : {prenom}, Ticket : {typeOperation}-{numeroAttribue}";
            listeClients.Add(clientInfo);

            while (true)
            {
                Console.Write("Souhaitez-vous effectuer une autre opération ? (o/n) : ");
                string reponse = Console.ReadLine().ToLower();

                if (reponse == "o")
                {
                    break;
                }
                else if (reponse == "n")
                {
                    SauvegarderNumeros();
                    SauvegarderListeClients();
                    AfficherListeClients();
                    Console.WriteLine("Merci d'avoir utilisé le distributeur de tickets. Au revoir !");
                    return;
                }
                else
                {
                    Console.WriteLine("Erreur : Veuillez saisir 'o' pour oui ou 'n' pour non.");
                }
            }
        }
    }

    private static int AttribuerNumero(string typeOperation)
    {
        numerosTickets[typeOperation]++;
        return numerosTickets[typeOperation];
    }

    private static string DemanderPrenom()
    {
        while (true)
        {
            Console.Write("Veuillez entrer votre prénom : ");
            string prenom = Console.ReadLine().Trim();

            if (!string.IsNullOrEmpty(prenom))
            {
                return prenom;
            }
            Console.WriteLine("Erreur : Le prénom ne peut pas être vide.");
        }
    }

    private static string DemanderNom()
    {
        while (true)
        {
            Console.Write("Veuillez entrer votre nom : ");
            string nom = Console.ReadLine().Trim();

            if (!string.IsNullOrEmpty(nom))
            {
                return nom;
            }
            Console.WriteLine("Erreur : Le nom ne peut pas être vide.");
        }
    }

    private static string DemanderNumeroCarte()
    {
        while (true)
        {
            Console.Write("Veuillez entrer votre numéro de carte bancaire (sans espaces) : ");
            string numeroCarte = Console.ReadLine().Trim();

            if (!string.IsNullOrEmpty(numeroCarte) && long.TryParse(numeroCarte, out _))
            {
                return numeroCarte;
            }
            Console.WriteLine("Erreur : Le numéro de carte bancaire doit être un nombre sans espaces.");
        }
    }

    private static void ChargerNumeros()
    {
        if (File.Exists(fichierNumeros))
        {
            string[] lignes = File.ReadAllLines(fichierNumeros);
            foreach (string ligne in lignes)
            {
                string[] parties = ligne.Split('-');
                if (parties.Length == 2 && numerosTickets.ContainsKey(parties[0]))
                {
                    numerosTickets[parties[0]] = int.Parse(parties[1]);
                }
            }
        }
    }

    private static void ChargerListeClients()
    {
        if (File.Exists(fichierClients))
        {
            listeClients.AddRange(File.ReadAllLines(fichierClients));
        }
    }

    private static void SauvegarderNumeros()
    {
        List<string> lignes = new List<string>();
        foreach (var kvp in numerosTickets)
        {
            lignes.Add($"{kvp.Key}-{kvp.Value}");
        }
        File.WriteAllLines(fichierNumeros, lignes);
    }

    private static void SauvegarderListeClients()
    {
        File.WriteAllLines(fichierClients, listeClients);
    }

    private static void AfficherListeClients()
    {
        Console.WriteLine("\nListe des clients servis :");
        foreach (string client in listeClients)
        {
            Console.WriteLine(client);
        }
    }
}

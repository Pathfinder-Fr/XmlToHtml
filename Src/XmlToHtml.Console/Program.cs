namespace XmlToHtml
{
    using System.Linq;
    using XmlToHtml.Commands;

    class Program
    {

        static int Main(string[] args)
        {
            string action;

            if (args.Length == 0)
            {
                action = string.Empty;
            }
            else
            {
                action = args[0].ToLowerInvariant();

                // Suppression premier paramètre
                args = args.Skip(1).ToArray();
            }

            CommandBase command = null;

            switch (action)
            {
                case "generate":
                    command = new ExportCommand();
                    break;

                case "customize":
                    command = new CustomizeCommand();
                    break;

                case "cleanurl":
                    command = new CleanUrlCommand();
                    break;

                case "create":
                    command = new CreateCommand();
                    break;

                default:
                    System.Console.WriteLine("ERREUR : action '{0}' inconnue. Liste des actions possibles :", action);
                    System.Console.WriteLine(" - generate : Génère les pages du DRP");
                    System.Console.WriteLine(" - customize : Génère la version personnalisée d'une page déjà existante");
                    System.Console.WriteLine(" - create : Créé une nouvelle page personnalisée");
                    return 1;
            }

            return command.Execute(args);
        }
    }
}

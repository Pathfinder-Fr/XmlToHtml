namespace PathfinderFr.XmlToHtml.Commands
{
    using System;
    using System.IO;
    using System.Text;

    class CustomizeCommand : GeneratorCommandBase
    {
        private string overridesPath;

        public override int Execute(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("ERREUR : Vous devez indiquer au moins un nom de fichier à personnaliser");
                return 4;
            }

            base.Execute(args);

            if (!Directory.Exists(overridesPath))
            {
                System.Console.WriteLine("Création du dossier destination {0}", overridesPath);
                Directory.CreateDirectory(overridesPath);
            }

            EnumerateInputs(f => CustomizePage(f));

            return 0;
        }

        protected override void ReadArgument(string argName, string value)
        {
            switch (argName)
            {
                case "v":
                    this.overridesPath = value;
                    break;

                default:
                    base.ReadArgument(argName, value);
                    break;
            }
        }

        private void CustomizePage(string file)
        {
            var fileName = Path.GetFileName(file);

            System.Console.Write("Création du fichier personnalisé {0}... ", fileName);

            var outPath = Path.Combine(this.overridesPath, fileName);


            WikiPage page = null;

            using (var reader = new StreamReader(file))
            {
                try
                {
                    page = (WikiPage)Serializer.Deserialize(reader);
                    page.FileName = fileName;
                }
                catch (Exception)
                {
                }
            }

            if (page == null)
            {
                System.Console.WriteLine("ERREUR (Format incorrect)");
                return;
            }
            
            var htmlBody = PrepareBody(page.HtmlBody);

            File.WriteAllText(outPath, htmlBody, Encoding.UTF8);
            System.Console.WriteLine("OK");
        }
    }
}

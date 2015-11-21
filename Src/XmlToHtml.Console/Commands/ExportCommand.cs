namespace XmlToHtml.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    class ExportCommand : GeneratorCommandBase
    {
        private const string DefaultEncoding = "utf-8";

        private List<string> overrides = new List<string>();

        private string templatePath = "template.html";

        private string outPath = "Out";

        private string overridesPath = "Overrides";

        /// <summary>
        /// Extension du fichier de template, incluant le ".". Exemple : ".html"
        /// </summary>
        private string templateExtension;

        private string template;

        private string encoding = DefaultEncoding;

        private Encoding fileEncoding;

        public override int Execute(string[] args)
        {
            var result = base.Execute(args);

            if (!File.Exists(templatePath))
            {
                Console.WriteLine("ERREUR : Le fichier template {0} n'existe pas.", templatePath);
                return 2;
            }

            if (!Directory.Exists(outPath))
            {
                Console.WriteLine("Création du dossier destination {0}", outPath);
                Directory.CreateDirectory(outPath);
            }

            this.templateExtension = Path.GetExtension(this.templatePath);
            Console.WriteLine("Format fichiers de remplacements : {0}", "*" + templateExtension);
            if (Directory.Exists(overridesPath))
            {
                this.overrides = new List<string>(Directory.GetFiles(overridesPath, "*" + templateExtension));
            }

            Console.WriteLine("Nombre de fichiers de remplacements : {0}", overrides.Count);

            System.Console.WriteLine("Encoding de lecture : {0}", this.encoding);
            this.fileEncoding = Encoding.GetEncoding(this.encoding);

            this.template = File.ReadAllText(templatePath, Encoding.UTF8);

            this.EnumerateInputs(f => ExportPage(f));

            return 0;
        }

        protected override void ReadArgument(string argName, string value)
        {
            switch (argName)
            {
                case "t":
                    this.templatePath = value;
                    System.Console.WriteLine("Chemin template : {0}", this.template);
                    break;

                case "o":
                    this.outPath = value;
                    System.Console.WriteLine("Chemin sortie : {0}", this.outPath);
                    break;

                case "v":
                    this.overridesPath = value;
                    System.Console.WriteLine("Chemin remplacements : {0}", this.overridesPath);
                    break;

                case "e":
                    this.encoding = value;
                    System.Console.WriteLine("Encoding : {0}", this.encoding);
                    break;

                default:
                    base.ReadArgument(argName, value);
                    break;
            }
        }

        private void ExportPage(string filePath)
        {
            var fileName = Path.GetFileName(filePath) ?? filePath;

            // Chemin du fichier éventuel de remplacement
            var overrideFileName = Path.GetFileNameWithoutExtension(fileName) + this.templateExtension;
            overrideFileName = Path.Combine(this.overridesPath, overrideFileName);

            System.Console.Write("Export du fichier {0}...", fileName);

            WikiPage page = null;

            if (!File.Exists(filePath))
            {
                System.Console.WriteLine("ERREUR (Fichier inexistant)");
                return;
            }
            else
            {
                using (var reader = new StreamReader(filePath, this.fileEncoding))
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
            }

            if (page == null)
            {
                System.Console.WriteLine("ERREUR (Format incorrect)");
                return;
            }

            var htmlBody = page.HtmlBody.Trim();

            if (File.Exists(overrideFileName))
            {
                // Remplacement du contenu par celui du fichier de remplacement
                htmlBody = File.ReadAllText(overrideFileName, Encoding.UTF8);

                overrides.Remove(overrideFileName);
            }
            else
            {
                htmlBody = this.PrepareBody(htmlBody);
            }

            string output = this.template
                .Replace("{Name}", page.Name)
                .Replace("{Title}", page.Title)
                .Replace("{Body}", htmlBody)
                .Replace("{Version}", page.Version.ToString())
                .Replace("{Modified}", page.LastModified.ToString("dd MMM yyyy HH:mm"))
                .Replace(Environment.NewLine, "\n")
                ;

            // On utilise l'extension du template comme extension du fichier en sortie
            var fileNameBase = Path.GetFileNameWithoutExtension(fileName);
            var templateExtension = Path.GetExtension(this.templatePath);
            var outFileName = fileNameBase + templateExtension;

            string fileOutPath = Path.Combine(this.outPath, outFileName);

            string oldCrc;
            string newCrc;
            bool changed = CheckIsChanged(output, fileOutPath, out oldCrc, out newCrc);

            if (changed)
            {
                File.WriteAllText(fileOutPath, output, Encoding.UTF8);
                System.Console.WriteLine("OK ({0} => {1})", oldCrc, newCrc);
            }
            else
            {
                System.Console.WriteLine("NOT MODIFIED");
            }
        }

        private static bool CheckIsChanged(string content, string filePath, out string oldHash, out string newHash)
        {
            bool changed = true;

            newHash = string.Empty;
            oldHash = string.Empty;

            if (File.Exists(filePath))
            {
                var bytes = Encoding.UTF8.GetBytes(content);

                using (Crc32 crc = new Crc32())
                {
                    foreach (var hashByte in crc.ComputeHash(bytes))
                    {
                        newHash += hashByte.ToString("x2");
                    }

                    var oldBytes = Encoding.UTF8.GetBytes(File.ReadAllText(filePath, Encoding.UTF8));

                    foreach (var hashByte in crc.ComputeHash(oldBytes))
                    {
                        oldHash += hashByte.ToString("x2");
                    }
                }

                if (oldHash.Equals(newHash, StringComparison.Ordinal))
                {
                    changed = false;
                }
            }

            return changed;
        }
    }
}
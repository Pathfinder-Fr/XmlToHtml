namespace XmlToHtml.Commands
{
    using System.Linq;
    using System.IO;
    using System.Xml;

    class CreateCommand : CommandBase
    {
        private string overridesPath = "Overrides";

        private string specificsPath = "Specifics";

        private const string DummyContent = "<!-- Tapez votre contenu HTML dans ce fichier -->";

        public override int Execute(string[] args)
        {
            base.Execute(args);

            var name = this.Inputs[0];
            var title = string.Join(" ", this.Inputs.Skip(1));

            var page = new WikiPage { Title = title };

            using (var writer = XmlWriter.Create(Path.Combine(specificsPath, name + ".xml"), new XmlWriterSettings { Indent = true }))
            {
                Serializer.Serialize(writer, page);
            }

            File.WriteAllText(Path.Combine(overridesPath, name + ".xml"), DummyContent);

            return 0;
        }

        protected override void ReadArgument(string argName, string value)
        {
            switch (argName)
            {
                case "v":
                    this.overridesPath = value;
                    break;

                case "s":
                    this.specificsPath = value;
                    break;
            }
        }
    }
}
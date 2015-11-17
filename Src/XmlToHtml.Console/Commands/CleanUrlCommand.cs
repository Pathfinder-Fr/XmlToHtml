using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace PathfinderFr.XmlToHtml.Commands
{
    class CleanUrlCommand : CommandBase
    {
        public override int Execute(string[] args)
        {
            base.Execute(args);

            base.EnumerateInputs(s => CleanPage(s));

            return 0;
        }

        private static void CleanPage(string filePath)
        {
            var fileName = Path.GetFileName(filePath);

            WikiPage page = null;

            using (var reader = new StreamReader(filePath, Encoding.Default))
            {
                try
                {
                    page = (WikiPage)Serializer.Deserialize(reader);
                }
                catch (Exception) { }
            }

            if (page == null)
            {
                System.Console.WriteLine("ERREUR (Format incorrect)");
                return;
            }

            var content = WebUtility.HtmlDecode(page.Body);

            page.Body = Regex.Replace(content, "href=\"([^\"]+)\"", HrefMatch, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            using (var writer = new StreamWriter(filePath, false, Encoding.Default))
            {
                try
                {
                    Serializer.Serialize(writer, page);
                }
                catch (Exception) { }
            }
        }

        private static string HrefMatch(Match match)
        {
            var target = match.Groups[1].Value;

            var dashIndex = target.IndexOf("#");

            if (dashIndex == 0)
            {
                target = target.ToUpperInvariant();
            }
            else if (dashIndex != -1 && dashIndex < target.Length - 1)
            {
                target = target.Substring(0, dashIndex + 1) + target.Substring(dashIndex + 1).ToUpperInvariant();
            }

            var escapedTarget = Uri.EscapeUriString(target);

            return string.Format("href=\"{0}\"", escapedTarget);
        }
    }
}

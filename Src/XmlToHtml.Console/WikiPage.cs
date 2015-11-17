namespace PathfinderFr.XmlToHtml
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Xml.Serialization;

    [XmlType("wikiPage")]
    [XmlRoot("wikiPage")]
    public class WikiPage
    {
        private DateTime lastModified;

        private string name;

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlArray("categories")]
        [XmlArrayItem("category")]
        public string[] Categories { get; set; }

        [XmlElement("lastModified")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string LastModifiedText
        {
            get { return lastModified.ToString("u"); }
            set { lastModified = DateTime.ParseExact(value, "u", CultureInfo.InvariantCulture); }
        }

        [XmlIgnore]
        public DateTime LastModified
        {
            get { return lastModified; }
            set { lastModified = value; }
        }

        [XmlAttribute("version")]
        public int Version { get; set; }

        [XmlArray("inLinks")]
        [XmlArrayItem("link")]
        public string[] InLinks { get; set; }

        [XmlArray("outLinks")]
        [XmlArrayItem("link")]
        public string[] OutLinks { get; set; }

        [XmlIgnore]
        public string FileName { get; set; }

        [XmlElement("body")]
        public string Body { get; set; }

        public string Name
        {
            get
            {
                if (name == null)
                    name = Path.GetFileNameWithoutExtension(this.FileName);

                return name;
            }
        }

        public string HtmlBody
        {
            get
            {
                return System.Net.WebUtility.HtmlDecode(this.Body ?? string.Empty).Replace("<br />", "<br />\r\n");
            }
        }
    }
}
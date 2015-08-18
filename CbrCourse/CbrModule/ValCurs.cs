using System.Collections.Generic;
using System.Xml.Serialization;

namespace CbrModule
{
    public class ValCurs
    {
        [XmlAttribute("Date")]
        public string Date { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlElement("Valute")]
        public List<Valute> Items { get; set; }
    }
}
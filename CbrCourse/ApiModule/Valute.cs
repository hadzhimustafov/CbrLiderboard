using System.Xml.Serialization;

namespace ApiModule
{
    public class Valute
    {
        [XmlElement("NumCode")]
        public string NumCode { get; set; }
        [XmlElement("CharCode")]
        public string CharCode { get; set; }
        [XmlElement("Nominal")]
        public string Nominal { get; set; }
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("Value")]
        public string Value { get; set; }
        [XmlAttribute("ID")]
        public string ID { get; set; }
    }
}
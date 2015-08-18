using System.Globalization;
using System.Xml.Serialization;

namespace ApiModule
{
    public class Record
    {
        [XmlAttribute("Date")]
        public string Date { get; set; }
        [XmlAttribute("Id")]
        public string Id { get; set; }
        [XmlElement("Nominal")]
        public int Nominal { get; set; }
        [XmlIgnore]
        public decimal Value { get; set; }

        [XmlElement("Value")]
        public string ValueFormatted
        {
            get { return Value.ToString(); }
            set { Value = decimal.Parse(value, NumberStyles.Any ); }
        }
    }
}
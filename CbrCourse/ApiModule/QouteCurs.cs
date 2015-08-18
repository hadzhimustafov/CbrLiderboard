using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ApiModule
{
    [System.Xml.Serialization.XmlTypeAttribute("ValCurs")]
    public class QouteCurs
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }
        [XmlAttribute("DateRange1")]
        public string DateRange1 { get; set; }
        [XmlAttribute("DateRange2")]
        public string DateRange2 { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlElement("Record")]
        public List<Record> Items { get; set; }    
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ApiModule
{
    [System.Xml.Serialization.XmlTypeAttribute("ValCurs")]
        public class DailyCurs
    {
        [XmlAttribute("Date")]
        public string Date { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlElement("Valute")]
        public ObservableCollection<Valute> Items { get; set; }
    }
}

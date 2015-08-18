using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.UI.Popups;

namespace CbrModule
{
    public class CbrRequsetClass
    {
        public async Task<ValCurs> GetDailyResponce()
        {
            HttpClient client = new HttpClient();
            try
            {
                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(5);
                HttpResponseMessage response = await httpClient.GetAsync(new Uri("http://www.cbr.ru/scripts/XML_daily.asp")).ConfigureAwait(false);
                var data = await response.Content.ReadAsStreamAsync();
                var datas = await response.Content.ReadAsStringAsync();
                XmlReader xr = XmlReader.Create(data);
                var serializer = new XmlSerializer(typeof(ValCurs));
                var res = new List<Valute>();
                return (ValCurs) serializer.Deserialize(xr);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("{0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace));    
            }
            return null;
        }
    }
}

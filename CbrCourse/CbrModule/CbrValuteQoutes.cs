using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ApiModule;

namespace CbrModule
{
    public class CbrValuteQoutes : IRepositoryCache<QouteCurs>
    {
        public async Task<QouteCurs> GetValuteQoutesResponce(Valute valute, DateTime dateFrom = default(DateTime))
        {
            HttpClient client = new HttpClient();
            try
            {
                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(5);
                var uri = this.GenerateUri(valute, dateFrom == default(DateTime) ? DateTime.Now.AddMonths(-1) : dateFrom,DateTime.Now);
                HttpResponseMessage response = await httpClient.GetAsync(uri).ConfigureAwait(false);
                var data = await response.Content.ReadAsStreamAsync();
                XmlReader xr = XmlReader.Create(data);
                var serializer = new XmlSerializer(typeof(QouteCurs));
               var t = (QouteCurs)serializer.Deserialize(xr);
                return t;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("{0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace));
            }
            return new QouteCurs(){Name = "Во время запроса произошла ошибка"};
        }
        private Uri GenerateUri(Valute valute, DateTime starDate, DateTime endDate)
        {
            var startString = starDate.ToString("dd'/'MM'/'yyyy");
            var endString = endDate.ToString("dd'/'MM'/'yyyy");
            var query = string.Format("date_req1={0}&date_req2={1}&VAL_NM_RQ={2}", startString, endString, valute.ID);
            return new Uri("http://www.cbr.ru/scripts/XML_dynamic.asp?" + query);
        }

        public void UpdateCache()
        {
            throw new NotImplementedException();
        }

        public Task<QouteCurs> GetResponse(object valute = null, DateTime dateFrom = new DateTime())
        {
            Valute val = valute as Valute;
            if (val == null) throw new ArgumentNullException("valute");
            return this.GetValuteQoutesResponce(val, dateFrom);
        }
    }


}

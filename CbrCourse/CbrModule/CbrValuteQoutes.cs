using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Storage;
using ApiModule;

namespace CbrModule
{
    public class CbrValuteQoutes : IRepositoryCache<QouteCurs>
    {
        private static readonly StorageFolder Folder = Windows.Storage.ApplicationData.Current.LocalFolder;
        private const CreationCollisionOption Option = CreationCollisionOption.ReplaceExisting;
        public async Task<QouteCurs> GetValuteQoutesResponce(Valute valute, DateTime dateFrom = default(DateTime))
        {
            try
            {
                var fileToRead = await Folder.GetFileAsync(string.Concat(valute.ID,".xml"));
                if (!fileToRead.IsAvailable)
                {
                    this.UpdateCache(valute,dateFrom).ConfigureAwait(false);
                    return null;
                }
                var bytes = await fileToRead.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
                XmlReader xr = XmlReader.Create(bytes.AsStream());
                var serializer = new XmlSerializer(typeof(QouteCurs));
                return (QouteCurs)serializer.Deserialize(xr);
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                this.UpdateCache(valute, dateFrom).ConfigureAwait(false);
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("{0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace));
            }
            return null;
            //HttpClient client = new HttpClient();
            //try
            //{
            //    var httpClient = new HttpClient();
            //    httpClient.Timeout = TimeSpan.FromSeconds(5);
            //    var uri = this.GenerateUri(valute, dateFrom == default(DateTime) ? DateTime.Now.AddMonths(-1) : dateFrom,DateTime.Now);
            //    HttpResponseMessage response = await httpClient.GetAsync(uri).ConfigureAwait(false);
            //    var data = await response.Content.ReadAsStreamAsync();
            //    XmlReader xr = XmlReader.Create(data);
            //    var serializer = new XmlSerializer(typeof(QouteCurs));
            //   var t = (QouteCurs)serializer.Deserialize(xr);
            //    return t;
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(string.Format("{0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace));
            //}
            //return new QouteCurs(){Name = "Во время запроса произошла ошибка"};
        }
        private Uri GenerateUri(Valute valute, DateTime starDate, DateTime endDate)
        {
            var startString = starDate.ToString("dd'/'MM'/'yyyy");
            var endString = endDate.ToString("dd'/'MM'/'yyyy");
            var query = string.Format("date_req1={0}&date_req2={1}&VAL_NM_RQ={2}", startString, endString, valute.ID);
            return new Uri("http://www.cbr.ru/scripts/XML_dynamic.asp?" + query);
        }

        public async Task UpdateCache(object valute = null, DateTime dateFrom = default(DateTime))
        {
            Valute val = valute as Valute;
            if (val == null) throw new ArgumentNullException("valute");
            try
            {
                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(5);
                var uri = this.GenerateUri(val, dateFrom == default(DateTime) ? DateTime.Now.AddMonths(-1) : dateFrom, DateTime.Now);
               HttpResponseMessage response = await httpClient.GetAsync(uri).ConfigureAwait(false);
                var data = await response.Content.ReadAsByteArrayAsync();
                var fileToWrite = await Folder.CreateFileAsync(string.Concat(val.ID, ".xml"), Option);
                await Windows.Storage.FileIO.WriteBytesAsync(fileToWrite, data);
                if (CashUpdated != null)
                {
                    CashUpdated(this, null);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("{0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace));
            }
        }

        public Task<QouteCurs> GetResponse(object valute = null, DateTime dateFrom = new DateTime())
        {
            Valute val = valute as Valute;
            if (val == null) throw new ArgumentNullException("valute");
            return this.GetValuteQoutesResponce(val, dateFrom);
        }

        public event EventHandler CashUpdated;
    }


}

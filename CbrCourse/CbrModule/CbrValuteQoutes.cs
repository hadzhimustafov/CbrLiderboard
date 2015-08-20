using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
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
                var stream = await ReadFile(string.Concat(valute.ID, dateFrom.ToString("_ddMMyyyy"), ".xml")).ConfigureAwait(false);
                XmlReader xr = XmlReader.Create(stream);
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
        }

        private async Task<Stream> ReadFile(string filename)
        {
            var file = await Folder.GetFileAsync(filename);
            if (!file.IsAvailable)
            {
                this.UpdateCache().ConfigureAwait(false);
                return null;
            }
            var fs = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            var inStream = fs.GetInputStreamAt(0);
            var reader = new Windows.Storage.Streams.DataReader(inStream);
            await reader.LoadAsync((uint)fs.Size);
            var data = reader.ReadBuffer((uint)fs.Size);
            reader.DetachStream();
            return data.AsStream();
        }

        private async Task WriteFile(string filename, byte[] contents)
        {
            var fileToWrite = await Folder.CreateFileAsync(filename, Option);
            await Windows.Storage.FileIO.WriteBytesAsync(fileToWrite, contents);
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
                var data = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                await this.WriteFile(string.Concat(val.ID, dateFrom.ToString("_ddMMyyyy"), ".xml"), data).ConfigureAwait(false);
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

        public Task<QouteCurs> GetResponse(object valute = null, DateTime dateFrom = default(DateTime))
        {
            Valute val = valute as Valute;
            if (val == null) throw new ArgumentNullException("valute");
            return this.GetValuteQoutesResponce(val, dateFrom);
        }

        public event EventHandler CashUpdated;
    }


}

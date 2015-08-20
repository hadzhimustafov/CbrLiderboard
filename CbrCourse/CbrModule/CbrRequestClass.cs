using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.UI.Popups;
using ApiModule;

namespace CbrModule
{
    public class CbrRequestClass : IRepositoryCache<DailyCurs>
    {
        const string FileName = "Daily.xml";
        private static readonly StorageFolder Folder = Windows.Storage.ApplicationData.Current.LocalFolder;
        private const CreationCollisionOption Option = CreationCollisionOption.ReplaceExisting;
        public async Task<DailyCurs> GetDailyResponce()
        {
            try
            {
                XmlReader xr = XmlReader.Create(await this.ReadFile(FileName).ConfigureAwait(false));
                var serializer = new XmlSerializer(typeof(DailyCurs));
                var res = new List<Valute>();
                return (DailyCurs)serializer.Deserialize(xr);
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                this.UpdateCache().ConfigureAwait(false);
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
        public async Task UpdateCache(object valute = null, DateTime dateFrom = default(DateTime))
        {
            try
            {
                HttpClient client = new HttpClient();
                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(5);
                HttpResponseMessage response =
                    await
                        httpClient.GetAsync(new Uri("http://www.cbr.ru/scripts/XML_daily.asp"))
                            .ConfigureAwait(false);
                var data = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                await this.WriteFile(FileName, data).ConfigureAwait(false);
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

        public Task<DailyCurs> GetResponse(object valute = null, DateTime dateFrom = default(DateTime))
        {
            return this.GetDailyResponce();
        }

        public event EventHandler CashUpdated;
    }
}

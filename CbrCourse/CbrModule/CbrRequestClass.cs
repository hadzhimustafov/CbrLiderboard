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
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
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
                var fileToRead = await Folder.GetFileAsync(FileName);
                if (!fileToRead.IsAvailable)
                {
                    this.UpdateCache().ConfigureAwait(false);
                    return null;
                }
                var bytes = await fileToRead.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
                XmlReader xr = XmlReader.Create(bytes.AsStream());
                var serializer = new XmlSerializer(typeof (DailyCurs));
                var res = new List<Valute>();
                return (DailyCurs) serializer.Deserialize(xr);
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
                    var data = await response.Content.ReadAsByteArrayAsync();
                    var fileToWrite = await Folder.CreateFileAsync(FileName, Option);
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

        public Task<DailyCurs> GetResponse(object valute = null, DateTime dateFrom = new DateTime())
        {
            return this.GetDailyResponce();
        }

        public event EventHandler CashUpdated;
    }
}

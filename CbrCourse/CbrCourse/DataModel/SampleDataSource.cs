using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// Модель данных, определяемая этим файлом, служит типичным примером строго типизированной
// по умолчанию.  Имена свойств совпадают с привязками данных из стандартных шаблонов элементов.
//
// Приложения могут использовать эту модель в качестве начальной точки и добавлять к ней дополнительные элементы или полностью удалить и
// заменить ее другой моделью, соответствующей их потребностям. Использование этой модели позволяет повысить качество приложения 
// скорость реагирования, инициируя задачу загрузки данных в коде программной части для App.xaml, если приложение 
// запускается впервые.
using ApiModule;
using CbrCourse.DataModel;
using CbrModule;

namespace CbrCourse.Data
{
    /// <summary>
    /// Создает коллекцию групп и элементов с содержимым, считываемым из статического JSON-файла.
    /// 
    /// SampleDataSource инициализируется данными, считываемыми из статического JSON-файла, включенного в 
    /// проект.  Предоставляет пример данных как во время разработки, так и во время выполнения.
    /// </summary>
    public sealed class SampleDataSource
    {
       
        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
        }

        public async Task<SampleDataGroup> GetGroupAsync()
        {
            await this.GetSampleDataAsync();

            return this.Group;
        }

        public async Task<Valute> GetItemAsync(string uniqueId)
        {
            await this.GetSampleDataAsync();
            // Для небольших наборов данных можно использовать простой линейный поиск
            var matches = this.Group.Items.Where((item) => item.ID.Equals(uniqueId));
            return matches.FirstOrDefault();
        }

        private async Task GetSampleDataAsync()
        {
            CbrRequestClass cbrRequest = new CbrRequestClass();
            var r = await cbrRequest.GetDailyResponce().ConfigureAwait(false);
            if (r == null) return;
            SampleDataGroup group = new SampleDataGroup(string.Format("Котировки: \"{0}\"",r.Name),r.Date.ToString());
            foreach (var valute in r.Items)
            {
                group.Items.Add(valute);
            }
            this._group = group;
        }

        public Task UpdateDataAsync()
        {
           return this.GetSampleDataAsync();
        }
    }

    public sealed class QoutesDataSource
    {
        private readonly Valute _valute;
        private readonly IRepositoryCache<QouteCurs> _qouteRepository;

        public QoutesDataSource(Valute valute, IRepositoryCache<QouteCurs> qouteRepository)
        {
            _valute = valute;
            _qouteRepository = qouteRepository;
        }

        private QouteCurs qouteCurs;
        public QouteCurs QouteCurs
        {
            get { return this.qouteCurs; }
        }

        public async Task<QouteCurs> GetGroupAsync()
        {
            await this.GetQouteCursAsync();

            return this.QouteCurs;
        }

        public async Task<Record> GetItemAsync(string uniqueId)
        {
            await this.GetQouteCursAsync();
            var matches = this.QouteCurs.Items.Where((item) => item.Id.Equals(uniqueId));
            return matches.FirstOrDefault();
        }

        private async Task GetQouteCursAsync()
        {
            CbrValuteQoutes cbrRequset = new CbrValuteQoutes();
            var r = await cbrRequset.GetValuteQoutesResponce(_valute).ConfigureAwait(false);
            if (r == null) return;
            this.qouteCurs = r;
        }

        public Task UpdateDataAsync()
        {
            return this.GetQouteCursAsync();
        }
    }
}
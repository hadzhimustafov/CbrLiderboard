using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Controls.Primitives;
using ApiModule;
using CbrCourse.Data;

namespace CbrCourse.Common
{
    public class MainViewModel : BaseViewModel
    {
        private readonly Func<Valute, ItemViewModel> _itemFactory;
        private ObservableCollection<Valute> _items;
        private readonly IRepositoryCache<DailyCurs> dataSource;
        private readonly ICacheUpdater _cacheUpdater;
        private DailyCurs dailyCurs;
        private ItemViewModel selectedItem;
        private CacheUpdateInterval _selectedInterval;

        public MainViewModel(Func<Valute, ItemViewModel> itemFactory, IRepositoryCache<DailyCurs> dataSource, ICacheUpdater cacheUpdater)
        {
            _itemFactory = itemFactory;
            this.dataSource = dataSource;
            _cacheUpdater = cacheUpdater;
            this.dataSource.CashUpdated += CashUpdated;
            if (!cacheUpdater.IsEnabled)
                cacheUpdater.Start();
        }

        void CashUpdated(object sender, EventArgs e)
        {
            this.UpdateGroupAsync();
        }

        public ObservableCollection<Valute> Items
        {
            get { return dailyCurs!=null? dailyCurs.Items : null; }
        }

        public string Title
        {
            get { return this.dailyCurs.Name; }
        }

        public string Date
        {
            get { return dailyCurs.Date; }
        }
        public ItemViewModel SelectedItem
        {
            get { return this.selectedItem; }
        }

        public Array CacheUpdateInterval
        {
            get { return Enum.GetValues(typeof (CacheUpdateInterval)); }
        }

        public CacheUpdateInterval SelectedInterval
        {
            get { return _selectedInterval; }
            set
            {
                if (Equals(value, _selectedInterval)) return;
                _selectedInterval = value;
                OnPropertyChanged();
            }
        }

        public  string LastUpdate
        {
            get
            {
                DateTime lastUpdateTimeAsync = _cacheUpdater.GetLastUpdateTimeAsync();
                if (lastUpdateTimeAsync == default (DateTime))
                    return "Кэш еще не обновлялся";
                return string.Format("Последнее обновление: {0}", lastUpdateTimeAsync.ToString("T"));
            }
        }

        public async void UpdateGroupAsync()
        {
            this.dailyCurs = await dataSource.GetResponse();
            this.OnPropertyChanged("Items");
            this.OnPropertyChanged("LastUpdate");
        }

        public async void UpdateSelectedItem(string s)
        {
            var selectedItem = this.dailyCurs.Items.FirstOrDefault(x => x.ID==s);
            this.selectedItem = _itemFactory(selectedItem);
            this.OnPropertyChanged("SelectedItem");
        }
    }

   
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Primitives;
using ApiModule;
using CbrCourse.Data;
using CbrCourse.DataModel;

namespace CbrCourse.Common
{
    public class MainViewModel : BaseViewModel
    {
        private readonly Func<Valute, ItemViewModel> _itemFactory;
        private ObservableCollection<Valute> _items;
        private SampleDataSource dataSource;
        private SampleDataGroup dataGroup;
        private ItemViewModel selectedItem;

        public MainViewModel(Func<Valute, ItemViewModel> itemFactory)
        {
            _itemFactory = itemFactory;
            this.dataSource = new SampleDataSource();
        }

        public ObservableCollection<Valute> Items
        {
            get { return dataGroup!=null? dataGroup.Items : null; }
        }

        public string Title
        {
            get { return this.dataGroup.Title; }
        }

        public string Date
        {
            get { return dataGroup.Date; }
        }
        public ItemViewModel SelectedItem
        {
            get { return this.selectedItem; }
        }
        public async void UpdateGroupAsync()
        {
            await this.dataSource.UpdateDataAsync();
            this.dataGroup = dataSource.Group;
            this.OnPropertyChanged("Items");
        }

        public async void UpdateSelectedItem(string s)
        {
            var selectedItem = await dataSource.GetItemAsync(s);
            this.selectedItem = _itemFactory(selectedItem);
            this.OnPropertyChanged("SelectedItem");
        }
    }
}

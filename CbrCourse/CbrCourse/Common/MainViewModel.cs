using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private DailyCurs dataGroup;
        private ItemViewModel selectedItem;

        public MainViewModel(Func<Valute, ItemViewModel> itemFactory, IRepositoryCache<DailyCurs> dataSource)
        {
            _itemFactory = itemFactory;
            this.dataSource = dataSource;
            this.dataSource.CashUpdated += CashUpdated;
        }

        void CashUpdated(object sender, EventArgs e)
        {
            this.UpdateGroupAsync();
            if (selectedItem!=null)
            {
                this.UpdateSelectedItem(selectedItem.Id);
            }
        }

        public ObservableCollection<Valute> Items
        {
            get { return dataGroup!=null? dataGroup.Items : null; }
        }

        public string Title
        {
            get { return this.dataGroup.Name; }
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
            this.dataGroup = await dataSource.GetResponse();
            this.OnPropertyChanged("Items");
        }

        public async void UpdateSelectedItem(string s)
        {
            var selectedItem = this.dataGroup.Items.FirstOrDefault(x => x.ID==s);
            this.selectedItem = _itemFactory(selectedItem);
            this.OnPropertyChanged("SelectedItem");
        }
    }
}

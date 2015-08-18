using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiModule;
using CbrCourse.Data;


namespace CbrCourse.Common
{
    public class ItemViewModel : BaseViewModel
    {
        private readonly Valute _selectedItem;
        private string _name;
        private string _charCode;
        private string _id;
        private string _nominal;
        private string _numCode;
        private string _value;
        private QoutesDataSource qoutesDataSource;
        private ChartViewModel chartViewModel;

        public ItemViewModel(Valute selectedItem, Func<Valute, QoutesDataSource> qoutesFactory)
        {
            _name = selectedItem.Name;
            _charCode = selectedItem.CharCode;
            _id = selectedItem.ID;
            _nominal = selectedItem.Nominal;
            _numCode = selectedItem.NumCode;
            _value = selectedItem.Value;
            qoutesDataSource = qoutesFactory(selectedItem);
            this.UpdateQoutesAsync();
        }

        public string Name
        {
            get { return _name; }
        }

        public string CharCode
        {
            get { return _charCode; }
        }

        public string Id
        {
            get { return _id; }
        }

        public string Nominal
        {
            get { return _nominal; }
        }

        public string NumCode
        {
            get { return _numCode; }
        }

        public string Value
        {
            get { return _value; }
        }

        public async void UpdateQoutesAsync()
        {
            QouteCurs qouteCurs = await this.qoutesDataSource.GetGroupAsync();
            this.chartViewModel = new ChartViewModel(new ObservableCollection<Record>(qouteCurs.Items), this.Name);
            this.OnPropertyChanged("ChartViewModel");
        }

        public ChartViewModel ChartViewModel
        {
            get { return this.chartViewModel; }
        }
    }
    // Create a ViewModel
    public sealed class ChartViewModel : BaseViewModel
    {
        private readonly ObservableCollection<Record> _records;
        private readonly string _caption;

        public ChartViewModel(ObservableCollection<Record> records, string caption)
        {
            _records = records;
            _caption = caption;
            this.OnPropertyChanged("Collection");
        }

        public string Caption
        {
            get { return _caption; }
        }
        public ObservableCollection<Record> Collection
        {
            get { return _records; }
        }
    }

}

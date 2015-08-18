using System;
using System.Collections.ObjectModel;
using ApiModule;

namespace CbrCourse.DataModel
{
    /// <summary>
    /// Универсальная модель данных групп.
    /// </summary>
    public class SampleDataGroup
    {
        public SampleDataGroup(String title, String date)
        {
            this.Title = title;
            this.Date = date;
            this.Items = new ObservableCollection<Valute>();
        }

        public string Title { get; private set; }
        public string Date { get; private set; }
        public ObservableCollection<Valute> Items { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} от {1}", this.Title, this.Date);
        }
    }
}
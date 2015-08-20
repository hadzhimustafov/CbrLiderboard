using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using CbrCourse.Annotations;

namespace CbrCourse.Common
{
    public class BaseViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            CoreDispatcher coreDispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            if (coreDispatcher == null) return;

            coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            });
        }
    }
}
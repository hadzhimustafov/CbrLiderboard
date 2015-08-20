using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using ApiModule;

namespace CbrModule
{
    public class DefaultCacheUpdater : ICacheUpdater
    {
        private readonly IRepositoryCache<DailyCurs> dailyCache;
        private CacheUpdateInterval interval;
        DispatcherTimer updateTimer = new DispatcherTimer();
        private static DateTime lastUpdateDateTime;
        CoreDispatcher coreDispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

        public DefaultCacheUpdater(IRepositoryCache<DailyCurs> dailyCache)
        {
            this.dailyCache = dailyCache;
            this.SetInterval();
            updateTimer.Tick += TimerTick;
        }

        async void TimerTick(object sender, object e)
        {
            lastUpdateDateTime = DateTime.Now;
            await UpdateImmediatelyAsync().ConfigureAwait(false);
        }

        public async Task UpdateImmediatelyAsync()
        {
            await coreDispatcher.RunAsync(CoreDispatcherPriority.Low, this.SetInterval);
            await this.dailyCache.UpdateCache().ConfigureAwait(false);
            await coreDispatcher.RunAsync(CoreDispatcherPriority.Low, ()=>this.updateTimer.Start());
        }

        public Task SetUpdateIntervalAsync(CacheUpdateInterval newInterval)
        {
            this.interval = newInterval;
            return this.UpdateImmediatelyAsync();
        }

        public DateTime GetLastUpdateTimeAsync()
        {
            return lastUpdateDateTime;
        }

        public Task Start()
        {
            return coreDispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                this.SetInterval();
                this.updateTimer.Start();
            }).AsTask();
        }

        public Task Stop()
        {
            return coreDispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                this.SetInterval();
                this.updateTimer.Stop();
            }).AsTask();
        }

        public bool IsEnabled { get { return this.updateTimer.IsEnabled; }}

        private void SetInterval()
        {
            switch (interval)
            {
                case CacheUpdateInterval.EveryHalfMinutes:
                {
                    this.updateTimer.Stop();
                    this.updateTimer.Interval = TimeSpan.FromSeconds(30);
                    break;
                }
                case CacheUpdateInterval.EveryMinutes:
                {
                    this.updateTimer.Stop();
                    this.updateTimer.Interval = TimeSpan.FromMinutes(1);
                    break;
                }
                case CacheUpdateInterval.EveryHour:
                {
                    this.updateTimer.Stop();
                    this.updateTimer.Interval = TimeSpan.FromHours(1);
                    break;
                }
                case CacheUpdateInterval.EveryDay:
                {
                    this.updateTimer.Stop();
                    this.updateTimer.Interval = TimeSpan.FromDays(1);
                    break;
                }
            }
        }
    }
}
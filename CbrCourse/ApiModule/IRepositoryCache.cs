using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ApiModule
{
    /// <summary>
    /// Доступ к данным
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepositoryCache<T>
    {
        /// <summary>
        /// Обновляет закешированные данные
        /// </summary>
        /// <param name="valute"></param>
        /// <param name="dateFrom"></param>
        /// <returns></returns>
        Task UpdateCache(object valute = null, DateTime dateFrom = new DateTime());
        /// <summary>
        /// Возращает данные из кеша
        /// </summary>
        /// <param name="valute"></param>
        /// <param name="dateFrom"></param>
        /// <returns></returns>
        Task<T> GetResponse(object valute = null, DateTime dateFrom = default(DateTime));
        /// <summary>
        /// Событие о том, что кеш обновлен
        /// </summary>
        event EventHandler CashUpdated;
    }

    /// <summary>
    /// Автоматический обновлятор закешированных данных
    /// </summary>
    public interface ICacheUpdater
    {
        /// <summary>
        /// Обновить немедленно, сбросив таймер
        /// </summary>
        /// <returns></returns>
        Task UpdateImmediatelyAsync();
        /// <summary>
        /// Задать интервал, сбросить таймер
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        Task SetUpdateIntervalAsync(CacheUpdateInterval interval);
        /// <summary>
        /// Последнее обновление
        /// </summary>
        /// <returns></returns>
        DateTime GetLastUpdateTimeAsync();
        /// <summary>
        /// Запуск сервиса
        /// </summary>
        /// <returns></returns>
        Task Start();
        /// <summary>
        /// Остановить сервис
        /// </summary>
        /// <returns></returns>
        Task Stop();

        bool IsEnabled { get; }
    }
}

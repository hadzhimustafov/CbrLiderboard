using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModule
{
    public interface IRepositoryCache<T>
    {
        Task UpdateCache(object valute = null, DateTime dateFrom = new DateTime());
        Task<T> GetResponse(object valute=null, DateTime dateFrom=default(DateTime));
        event EventHandler CashUpdated;
    }
}

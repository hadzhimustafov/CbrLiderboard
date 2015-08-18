using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModule
{
    public interface IRepositoryCache<T>
    {
        void UpdateCache();
        Task<T> GetResponse(object valute=null, DateTime dateFrom=default(DateTime));
    }
}

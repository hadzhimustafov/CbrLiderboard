using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiModule;
using Autofac;

namespace CbrModule
{
    public class CbrModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CbrRequestClass>().As<IRepositoryCache<DailyCurs>>().SingleInstance();//Отдаем всем запросившим один экземпляр, чтобы всегда был актуальный кэш
            builder.RegisterType<CbrValuteQoutes>().As<IRepositoryCache<QouteCurs>>();
            builder.RegisterType<DefaultCacheUpdater>().As<ICacheUpdater>().SingleInstance();//Сервис обновления кэша должен быть для всего приложения
            base.Load(builder);
        }
    }
}

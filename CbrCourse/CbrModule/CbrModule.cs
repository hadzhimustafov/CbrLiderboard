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
            builder.RegisterType<CbrRequestClass>().As<IRepositoryCache<DailyCurs>>();
            builder.RegisterType<CbrValuteQoutes>().As<IRepositoryCache<QouteCurs>>();
            base.Load(builder);
        }
    }
}

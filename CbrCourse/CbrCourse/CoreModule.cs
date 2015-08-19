using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CbrCourse.Common;
using CbrCourse.Data;

namespace CbrCourse
{
    class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BaseViewModel>().AsSelf();
            builder.RegisterType<ItemViewModel>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<HubPage>().AsSelf();
            builder.RegisterType<ItemPage>().AsSelf();
            base.Load(builder);
        }
    }
}

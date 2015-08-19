

using Autofac;

namespace ApiModule
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Valute>().AsSelf();
            builder.RegisterType<DailyCurs>().AsSelf();
            base.Load(builder);
        }

        
    }
}

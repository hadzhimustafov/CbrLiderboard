

using Autofac;

namespace ApiModule
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DailyCurs>().AsSelf();
            base.Load(builder);
        }

        
    }
}

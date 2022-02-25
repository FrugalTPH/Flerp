using Autofac;
using Flerp.Client.Views;

namespace Flerp.Client
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainView>().As<IMainView>();
            //base.Load(builder);
        }
    }
}
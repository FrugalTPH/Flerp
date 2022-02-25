using Autofac;
using Flerp.Data.EmailCrawler;

namespace Flerp.Data
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailClient>().As<IEmailClient>();
            //base.Load(builder);
        }
    }
}
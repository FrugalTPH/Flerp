using Autofac;

namespace Flerp.Data.LocalDb
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DatabaseController>().As<IDatabaseController>();
            builder.RegisterType<Controller>().As<IController>();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
            //base.Load(builder);
        }
    }
}
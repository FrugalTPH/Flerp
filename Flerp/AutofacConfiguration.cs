using Autofac;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Flerp
{
    public class AutofacConfiguration
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            RegisterModulesFromFlerpAssemblies(builder);

            return builder.Build();
        }

        private static void RegisterModulesFromFlerpAssemblies(ContainerBuilder builder)
        {
            var flerpAssemblies = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)
                .GetFiles("Flerp.*.dll", SearchOption.TopDirectoryOnly)
                .Select(f => AssemblyName.GetAssemblyName(f.FullName))
                .Select(an => AppDomain.CurrentDomain.Load(an))
                .ToArray();          

            builder.RegisterAssemblyModules(flerpAssemblies);
        }
    }
}
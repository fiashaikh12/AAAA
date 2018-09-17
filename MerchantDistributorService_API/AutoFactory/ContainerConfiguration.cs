using Autofac;
using Autofac.Integration.WebApi;
using BusinessLogicLayer.Repository;
using Repository;
using System.Reflection;
using System.Web.Http;

namespace AutoFactory
{
    public class AutoFactoryConfiguration
    {
        private static IContainer container;
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }
        public static void Initialize(HttpConfiguration config,IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
        public static IContainer RegisterServices(ContainerBuilder builder)
        {
            //Register API controllers using assembly scanning.
            //builder.RegisterAssemblyTypes(Assembly.Load(nameof(BusinessLogicLayer))).
            //Where(t => t.Namespace.Contains("Repository.Interface"))
            //.As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest();
            builder.RegisterType<CommonRepository>().As<ICommonRepository>().InstancePerRequest();
            container = builder.Build();
            return container;
        }
    }
}
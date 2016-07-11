using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using EventDemoProject.Controllers;
using EventDemoProject.DataRepsitory;
using EventDemoProject.Models.DataContext;
using EventDemoProject.Repository;

namespace EventDemoProject
{
    public class AutofacConfig
    {
        public static IContainer RegisterDependencies()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterControllers(typeof (HomeController).Assembly);
            containerBuilder.RegisterApiControllers(typeof (HomeController).Assembly);
            
            //DataContext register.
            containerBuilder.RegisterType<EventDbContext>().As<DbContext>().InstancePerDependency();
            
            //Repository register
            containerBuilder.RegisterType<EventRepository>().As<IEventRepository>().InstancePerDependency();

            //Data-Repository register
            containerBuilder.RegisterGeneric(typeof(DataRepository<>))
                .As(typeof (IDataRepository<>))
                .InstancePerDependency();

            var container = containerBuilder.Build();
            //container.ActivateGlimpse();

            //This will set dependency resolver for MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //This will set dependency resolver for WebAPI
            var resolver = new AutofacWebApiDependencyResolver(container);
           GlobalConfiguration.Configuration.DependencyResolver = resolver;
            return container;
        }
    }
}
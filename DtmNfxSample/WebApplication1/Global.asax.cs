using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.Mvc;
using Dtmcli;
using Microsoft.Extensions.DependencyInjection;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            RegSomething();
        }

        private void RegSomething()
        {
            var builder = new ContainerBuilder();

            var dtmUrl = "http://localhost:36789";
            var services = new ServiceCollection();
            services.AddDtmcli(x =>
            {
                x.DtmUrl = dtmUrl;
                x.DtmTimeout = 8000;
                x.BranchTimeout = 8000;
            });

            builder.Populate(services); 
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}

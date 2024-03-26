
using Autofac;
using AutofacPOC.Services;

namespace AutofacPOC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var startup = new Startup(builder.Configuration);
            startup.ConfigureServices(builder.Services);

            builder.Host
                .UseServiceProviderFactory(new AutofacMultitenantServiceProviderFactory(startup.ConfigureMultitenantContainer))
                .ConfigureContainer<ContainerBuilder>((container) =>
                {
                    container.RegisterType<ProxyService>().As<IProxy>();
                    container.RegisterType<DefaultService>().As<ITenantService>();
                });

            var app = builder.Build();

            startup.Configure(app);

            app.Run();
        }
    }
}

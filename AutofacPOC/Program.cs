
using Autofac.Extensions.DependencyInjection;
using Autofac;
using AutofacPOC.Services;
using Microsoft.AspNetCore.Hosting;
using Autofac.Multitenant;

namespace AutofacPOC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host
                .UseServiceProviderFactory(new AutofacMultitenantServiceProviderFactory(Program.ConfigureMultitenantContainer))
                .ConfigureContainer<ContainerBuilder>((container) =>
                {
                    container.RegisterType<DefaultService>().As<ITenantService>();
                });

            builder.Services.AddAutofacMultitenantRequestServices();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        public static MultitenantContainer ConfigureMultitenantContainer(IContainer container)
        {
            // This is the MULTITENANT PART. Set up your tenant-specific stuff here.
            var strategy = new MyTenantIdentificationStrategy(container.Resolve<IHttpContextAccessor>());
            var mtc = new MultitenantContainer(strategy, container);
            mtc.ConfigureTenant("A", cb => cb.RegisterType<TenantAService>().As<ITenantService>());
            mtc.ConfigureTenant("B", cb => cb.RegisterType<TenantBService>().As<ITenantService>());
            return mtc;
        }
    }
}

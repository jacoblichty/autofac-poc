using Autofac;
using Autofac.Multitenant;
using AutofacPOC.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;

namespace AutofacPOC
{
    public class Startup
    {
        public IConfiguration Configuration
        {
            get;
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(
                    options =>
                    {
                        this.Configuration.Bind("AzureAdB2C", options);
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = "https://esrigamanonprod.b2clogin.com/96b23488-5884-4529-b72e-e9db4d6d7f32/v2.0/",
                            ValidAudience = "a48b6b76-530e-4309-ba18-e24cedf28548",
                        };
                        //options.Events = this.Container.ConfigureAuthenticationEvents();
                    },
                    options =>
                    {
                        this.Configuration.Bind("AzureAdB2C", options);
                    });

            services.AddAutofacMultitenantRequestServices();

            // Add services to the container.

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public MultitenantContainer ConfigureMultitenantContainer(IContainer container)
        {
            // This is the MULTITENANT PART. Set up your tenant-specific stuff here.
            var strategy = new MyTenantIdentificationStrategy(container.Resolve<IHttpContextAccessor>());
            var mtc = new MultitenantContainer(strategy, container);
            mtc.ConfigureTenant("A", cb => cb.RegisterType<TenantAService>().As<ITenantService>());
            mtc.ConfigureTenant("B", cb => cb.RegisterType<TenantBService>().As<ITenantService>());
            return mtc;
        }

        public void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}

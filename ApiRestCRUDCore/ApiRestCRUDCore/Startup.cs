using ApiRestCRUDCore.Data.Dapper.Repositories;
using ApiRestCRUDCore.Data.EF;
using ApiRestCRUDCore.Data.EF.Repositories;
using ApiRestCRUDCore.Domain.Contracts;
using Components.Sign;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiRestCRUDCore
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // CORS LIBERADO APENAS EM HOMOLOG
            services.AddCors(o => o.AddPolicy("ALLOW", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddMvc();

            services.AddDbContext<ApiRestCRUDCoreDataContext>(options => options
           .UseSqlServer(Configuration.GetConnectionString("ApiConnection")));

            Injection(services);

            new SigningConfigurations().ConfigurarAutenticacao(services, Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            DbInitializer.Initialize(app.ApplicationServices.
               GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);

            app.UseAuthentication();

            app.UseMvc();

        }

        public void Injection(IServiceCollection services)
        {

            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));

            services.AddTransient<IEmployeeRepository, EmployeeRepository>();

        }

    }
}

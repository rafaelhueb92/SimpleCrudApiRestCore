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
using Swashbuckle.AspNetCore.Swagger;

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

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Api Core Restfull CRUD",
                    Description = "A simple example ASP.NET Core Web API Rest With Dapper And EF, Created By Rafael Hueb",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Rafael Hueb",
                        Email = "rafaelhueb92@gmail.com",
                        Url = "https://www.instagram.com/ragahu92/"
                    }
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Core Restfull CRUD");
            });

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

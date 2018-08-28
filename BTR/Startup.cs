using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Http;

using BTR.Middleware;

namespace BTR
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
            services.AddCors();

            services.AddDbContext<BTR.DataAccess.BTRContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("BTRBranch"))
            );

            // config for user groups
            services.AddSingleton(cfg =>
            {
                var usrRights = new UserRights();
                Configuration.GetSection("UserRights:BTR").Bind(usrRights) ;
                return usrRights;
            });

            // config for email
            services.AddSingleton(cfg => {
                var emailCfg = new EmailConfig();
                Configuration.GetSection("EmailSettings:BTR").Bind(emailCfg);
                return emailCfg;
            });
            
            // used by functions like GetIdeaById and GetThemeById to loop to create their subtrees
            services.AddMvc()
              .AddJsonOptions(
                    options => options.SerializerSettings.ReferenceLoopHandling
                        = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options =>
                options.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
            );

            app.UseMvc();
        }
    }
}

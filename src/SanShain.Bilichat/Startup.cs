using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace SanShain.Bilichat
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Models.BilichatContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("mysql"));
            });

            services.AddDistributedRedisCache(options => {
                options.Configuration = Configuration.GetConnectionString("redis");
                options.InstanceName = "bilichat_";//不知道有啥用,只知道key会有这个前缀
            });

            services.AddHttpClient();
            services.AddSingleton<Services.CacheService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
            });
            app.UseMvc();
        }
    }
}

using Helpers.Interfaces;
using Helpers.Services;
using Interface.ServiceInterfaces;
using Interfaces.RepositoryInterfaces;
using Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models.DbModels;
using Models.HelperModels;
using Models.HelperModels.DependencyModels.Interfaces;
using Models.HelperModels.DependencyModels.Services;
using Repository.Base.Services;
using Repository.Services;
using Services.IdentityServices;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebApi
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
            services.AddDbContext<NewsContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityData.Identity.ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<NewsContext>()
                .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options => { })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = "mysite.com",
                    ValidateIssuer = true,
                    ValidIssuer = "mysite.com",
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("denemedenemedenemedeneme"))
                };
            });

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddSingleton(Configuration);
            services.AddSingleton<ICacheManager, CacheManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddSingleton<IServiceProviderAccessor, ServiceProviderAccessor>();

            services.AddScoped(typeof(IBaseGenericRepository<,,>), typeof(BaseGenericBaseRepository<,,>));
            services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
            services.AddScoped<INewsContextProvider, NewsContextProvider>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHttpContextAccessor httpContextAccessor, IServiceProviderAccessor serviceProviderAccessor, IHostingEnvironment env)
        {
            ServiceGetter.SetAccessor(httpContextAccessor);
            ServiceGetter.SetAccessor(serviceProviderAccessor);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

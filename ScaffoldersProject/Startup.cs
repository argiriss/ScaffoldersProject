using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScaffoldersProject.Data;
using ScaffoldersProject.Models;
using ScaffoldersProject.Hubs;
using ScaffoldersProject.Services;
using ScaffoldersProject.Models.services;
using Microsoft.AspNetCore.HttpOverrides;


namespace ScaffoldersProject
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        //Constructor with Depedency injection
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Adding new service for reading values of Appsettings.json file
            //After adding the service, we will read the values of 
            //Connection string, keys and value pairs.
            //We are going to use Constructor injection in every class we want 
            //reading the values
            services.AddSingleton<IConfiguration>(Configuration);

            //Identity database for user authentication.
            //Connection string in appsettings.json.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Main database for the project.
            //Connection string in appsettings.json.
            services.AddDbContext<MainDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MainConnection")));

            //Authentication
            services.AddIdentity<ApplicationUser, IdentityRole>( options =>
            {
                //Lockout Settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
               //options.Lockout.MaxFailedAccessAttempts = 3;

                //User Setting
                options.User.RequireUniqueEmail = true;

                //Sing In Settings
                options.SignIn.RequireConfirmedEmail = true;
                
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IProductRepository, EfProductRepository>();
            services.AddTransient<ICartRepository, EfCartRepository>();
            services.AddTransient<IOrderRepository, EfOrderRepository>();
            services.AddTransient<ISellRepository, EfSellRepository>();
            services.AddTransient<IWalletRepository, EfWalletRepository>();
            services.AddTransient<IOfferRepository, EfOfferRepository>();
            services.AddTransient<IAskRepository, EfAskRepository>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IWebApiFetch, WebApiFetch>();

            //Add cart service
            //services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));

            //SignalR can be used to add any sort of "real-time" web functionality 
            //to our ASP.NET application. 
            //Install from pm manager
            //PM>Install-Package Microsoft.AspNetCore.SignalR -Version 1.0.0-alpha2-final
            services.AddSignalR();

            services.AddMvc();

            services.Configure<AuthMessageSenderOptions>(Configuration);
            //Enabling sessions states with storing them into memory.
            //The addMemoryCash method call sets up the in-memory data store
            //The AddSession method registers the services used to access session data, 
            //and the UseSession method allows the session system to automatically 
            //associate requests with sessions when they arrive from the client.
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the
        //HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            
            app.UseSession();

            //When setting up a reverse proxy server, the authentication middleware needs 
            //UseForwardedHeaders to run first. This ordering ensures that the authentication 
            //middleware can consume the affected values and generate correct redirect URIs.
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();

            //app.UseFileServer();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("Chat");
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<MainHub>("Main");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //sets the user roles.We can add more from Initializer static class 
            //in data folder
            await Initializer.Initial(app);
        }
    }
}

using Hangfire;
using Hangfire.SQLite;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;

using ThreeS.Jobs;
using ThreeS.Routine.Data;

namespace ThreeS.Routine
{
    /// <summary>
    /// Defines the <see cref="Startup" />
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// The Configure
        /// </summary>
        /// <param name="app">The app<see cref="IApplicationBuilder"/></param>
        /// <param name="env">The env<see cref="IHostingEnvironment"/></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                ApplicationDbInitializer.SeedData(userManager, roleManager);
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseHangfireServer();
            app.UseHangfireDashboard("/jobs", new DashboardOptions()
            {
                DisplayStorageConnectionString = false,
                // Authorization = new[] { new CustomAuthorizeFilter() },
                DashboardTitle = "Scheduled Jobs Overview"
            });
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// The ConfigureServices
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging()).AddDistributedMemoryCache();
            // services.AddScoped<IDbConnection>(f => new SqlConnection(Configuration.GetConnectionString("WorkerConnetion")));
            services.AddTransient<IDbConnectionFactory, DapperDbConnectionFactory>();
            services.AddScoped<ISQLJobs, SQLJobs>();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.Configure<AppConfig>(options => Configuration.GetSection("AppConfig").Bind(options));
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            //services.AddDbContext<DataProtectionContext>(options =>
            //  options.UseSqlite("Data Source=dtprotection.sqlite3")).AddAntiforgery();
            // using Microsoft.AspNetCore.DataProtection;
            services.AddDataProtection();
            services.AddDefaultIdentity<ApplicationUser>().AddRoles<ApplicationRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddSingleton(new BackgroundJobServerOptions
            {
                WorkerCount = 2,
                ServerName = "TaskSvcServer"
            });
            services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer().UseSerilogLogProvider()
        .UseRecommendedSerializerSettings()
        .UseSQLiteStorage("Filename=ThreeSDB.sqlite3;", new SQLiteStorageOptions
        {
            TransactionIsolationLevel = System.Data.IsolationLevel.ReadCommitted
        }));
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                //options.LoginPath = "/Identity/Account/Login";
                //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            // Add the processing server as IHostedService
            // services.AddHangfireServer();

            services.AddMvc().AddRazorPagesOptions(options =>
            {
                // options.Conventions.AuthorizeFolder("/");
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
    }
}

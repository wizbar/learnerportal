using System;
using learner_portal.Helpers;
using learner_portal.Models;
using learner_portal.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using reCAPTCHA.AspNetCore;
using Rotativa.AspNetCore;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace learner_portal
{
    public class Startup
    {
        readonly string LearnerCorsPolicy = "_LearnerCorsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
 
        public IConfiguration Configuration { get; }  

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Email Server Configuration   
            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            
            //Add Cross-Site Policies
            services.AddCors(options =>
            {
                options.AddPolicy(name: LearnerCorsPolicy,
                    builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
            });

            //Configure Racaptcha
            var recaptcha = Configuration.GetSection("RecaptchaSettings");
            if (!recaptcha.Exists())
                throw new ArgumentException("Missing RecaptchaSettings in configuration.");

            services.Configure<RecaptchaSettings>(recaptcha);
            services.AddTransient<IRecaptchaService, RecaptchaService>();
            
            //Add Cookie Configurations for Authenticating Users
            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "LearnerIdentity.Cookie";
                config.LoginPath = "/Account/Login";
                config.LogoutPath = "/Account/Logout";
                config.AccessDeniedPath = "/Account/AccessDenied";
                config.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            });
            
            //Email Limits 
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
            
            //Configure Cookie Policy options 
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;

            });

            //Configure Toast and Notification Services for alerts 
            /*
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNToastNotifyToastr(new NToastNotify.ToastrOptions() { ProgressBar = false, PositionClass = ToastPositions.BottomRight }, new NToastNotify.NToastNotifyOption()
                {
                    ScriptSrc = "https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js?7.0.0.0",
                    StyleHref = "https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css?7.0.0.0"
                });
                */



            //Add Context Assessor Singleton 
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 
            //services.AddMemoryCache();
            services.AddMvc();
 
            // Add Database Context to access the database
            services.AddDbContext<LearnerContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DBConnection")),
                    ServiceLifetime.Transient).AddIdentity<Users, Roles>(
                    config =>
                    {
                        config.Password.RequiredLength = 7;
                        config.User.RequireUniqueEmail = true;
                        config.User.AllowedUserNameCharacters =
                            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    })
                .AddEntityFrameworkStores<LearnerContext>()
                .AddDefaultTokenProviders().AddDefaultUI();
            
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ILookUpService, LookUpService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "LearnerPortalIdentity.Cookie";
                options.LoginPath = $"/Account/Login";
                options.LogoutPath = $"/Account/Logout";
                options.AccessDeniedPath = $"/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            });

            
            //Enables Anonymous access to some pages.
            services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.AllowAnonymousToPage("/Account");
            });
            
            //Folders Configuration    
            var foldersConfigation = Configuration
                .GetSection("FoldersConfigation")
                .Get<FoldersConfigation>();
            services.AddSingleton(foldersConfigation);
            services.AddSession(options =>
            {
                options.Cookie.Name = ".LearnerPortal.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.IsEssential = true;
            });
            services.AddControllersWithViews();
            services.AddRazorPages();
 
            services.AddControllersWithViews().AddNewtonsoftJson(
                options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });
            ;
        }
 
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IHostingEnvironment _env)
        {
           
            
            loggerFactory.AddFile("Logs/learner_portal-{Date}.txt");
            
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/core-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
 
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseCookiePolicy();
            // app.UseNToastNotify();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                   );
              
            });
            RotativaConfiguration.Setup(_env, "Rotativa");
           }
    }
}
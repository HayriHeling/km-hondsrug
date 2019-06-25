using Eduria.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Eduria
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Creates session, cookies and adds authentication.
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //Add own DbContext and use Sql Server.
            services.AddDbContext<EduriaContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EduriaDevelopment")));
            //Add over services.
            services.Configure<AppSettingsService>(Configuration.GetSection("ConnectionStrings"));
            services.AddScoped<ExamResultService>();
            services.AddScoped<UserService>();
            services.AddScoped<ExamService>();
            services.AddScoped<AnalyticDefaultService>();
            services.AddScoped<QuestionService>();
            services.AddScoped<AnswerService>();
            services.AddScoped<TimeTableService>();
            services.AddScoped<UserEQLogService>();
            services.AddScoped<ExamQuestionService>();
            services.AddScoped<MediaSourceService>();
            services.AddScoped<ConfigsService>();
            services.AddScoped<DatabaseService>();
            services.AddScoped<TimeTableInformationService>();
            services.AddScoped<TimeTableInfoMediaSrcService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Index}/{id?}");   
            });
        }
    }
}

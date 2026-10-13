using GreenShop.Hubs;
using GreenShop.Models;
using GreenShop.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GreenShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotEnvFile.Load();
            var builder = WebApplication.CreateBuilder(args);
            var appSettingsSources = builder.Configuration.Sources
                .OfType<JsonConfigurationSource>()
                .Where(source => Path.GetFileName(source.Path ?? string.Empty)
                    .StartsWith("appsettings", StringComparison.OrdinalIgnoreCase))
                .ToList();
            foreach (var source in appSettingsSources)
            {
                builder.Configuration.Sources.Remove(source);
            }
            builder.Configuration.AddEnvironmentVariables();
            builder.Configuration.AddCommandLine(args);

            builder.Services.AddScoped<AuthServicecs>();
            builder.Services.AddScoped<EmailService>();

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("MyConnect");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'MyConnect' is missing. Set ConnectionStrings__MyConnect in .env or the system environment.");
            }

            builder.Services.AddDbContext<GreenShopContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            builder.Services.AddAuthentication(options =>
            {
                // Cấu hình mặc định để xác thực JWT
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

                // Cấu hình cho Google
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
   .AddJwtBearer(options =>
   {
       options.SaveToken = true;
       options.RequireHttpsMetadata = false;
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidAudience = builder.Configuration["JWT:ValidAudience"],
           ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
       };
   })
   .AddCookie() // Bắt buộc phải có để sử dụng Google Authentication
   .AddGoogle(options =>
   {
       options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
       options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
       options.Scope.Add("openid");
       options.Scope.Add("profile");
       options.Scope.Add("email");
       options.CallbackPath = "/signin-google";
   });

            builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Shop", "Shop");
            });
            builder.Services.AddSignalR();
            builder.Services.AddHttpContextAccessor();

            // Register the HttpClient service
            builder.Services.AddHttpClient();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseMiddleware<AdminCheckMiddleware>();


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapHub<ProductHubs>("/productHub");
            app.MapHub<CategoryHubs>("/categoryHub");
            // Redirect from "/" to "/Shop/Home"
            app.MapGet("/", context =>
            {
                context.Response.Redirect("/Shop");
                return Task.CompletedTask;
            });
            app.Run();
        }
    }
}

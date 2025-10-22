using GreenShop.Hubs;
using GreenShop.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSignalR();
            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<GreenShopContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnect"));
            });

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

            app.UseAuthorization();
            app.MapHub<ProductHubs>("/productHub");
            app.MapHub<CategoryHubs>("/categoryHub");
            app.MapRazorPages();

            app.MapGet("/", context =>
            {
                context.Response.Redirect("/Shop");
                return Task.CompletedTask;
            });

            app.Run();
        }
    }
}

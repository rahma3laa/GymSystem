using GymManagementBLL;
using GymManagementBLL.Classes;
using GymManagementBLL.Services.Interface;
using GymManagementDAL.Data.Context;
using GymManagementDAL.Data.DataSeed;
using GymManagementDAL.Repostiories.Classes;
using GymManagementDAL.Repostiories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace GymManagementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(Options =>
            {
                //App Setting
                //Options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["Default Connection"]);
                //Options.UseSqlServer(builder.Configuration["ConnectionStrings:Default Connection"]);

                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(X => X.AddProfile(new MappingProfiles()));
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            var app = builder.Build();


            #region Migrate Database - Data Seeding
            using var Scope = app.Services.CreateScope();
            var dbContext = Scope.ServiceProvider.GetRequiredService<GymDbContext>();
            
            var PendingMigrations= dbContext.Database.GetPendingMigrations();
            if (PendingMigrations?.Any() ?? false) 
                dbContext.Database.Migrate();
            GymDbContextSeeding.SeedData(dbContext);
            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

        

            // 👇 لازم يكون هنا
            app.UseStaticFiles();

       
            app.UseAuthorization();

            app.MapDefaultControllerRoute();

        

            app.UseHttpsRedirection();
            app.UseStaticFiles();
          
            app.UseRouting();

            app.MapControllers();

           

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id:int?}")
                .WithStaticAssets();

            app.MapControllerRoute(
                name: "Trainers",
                pattern: "Coach/{action}",
                defaults: new { controller = "Trainer" , action  = "Index"}

                );

            app.Run();
        }
    }
}

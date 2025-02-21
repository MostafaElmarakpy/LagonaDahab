using LagonaDahab.Application.Common.Interfaces;
using LagonaDahab.Infrastructure.Data;
using LagonaDahab.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenaricRepository<>), typeof(GenaricRepository<>));
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
var app = builder.Build();

#region Database Migration and Seeding

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<Program>();

    try
    {
        // Migrate the main database
        var context = services.GetRequiredService<ApplicationDbContext>();
        logger.LogInformation("Starting database migration");
        await context.Database.MigrateAsync();
        await ApplicationDbContextSeed.SeedAsync(context, loggerFactory);

        //var identityContext = services.GetRequiredService<AppIdentityDbContext>();
        //await identityContext.Database.MigrateAsync();
        //// Seed initial users
        //var userManager = services.GetRequiredService<UserManager<AppUser>>();
        //await AppIdentityDbContextSeed.SeedUserAsync(userManager);

        logger.LogInformation("Database Migrated successfully");
    }
    catch (Exception ex)
    {
        var loggers = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database migration");
    }
}

#endregion


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Villa}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

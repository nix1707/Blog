using Blog;
using Blog.Data;
using Blog.Data.FileManager;
using Blog.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;

})
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";

});

builder.Services.AddLogging();

builder.Services.AddTransient<IRepository, Repository>();
builder.Services.AddTransient<IFileManager, FileManager>();

builder.Services.AddMvc(options =>
{ 
    options.EnableEndpointRouting = false;
    options.CacheProfiles.Add("Monthly",new CacheProfile { Duration = (int)TimeSpan.FromDays(28).TotalSeconds});
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Seed>>();

await Seed.SeedData(context, userManager, roleManager, logger);

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseMvcWithDefaultRoute();
app.Run();

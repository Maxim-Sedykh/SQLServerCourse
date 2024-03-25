using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SQLServerCourse.Initializer;
using NLog.Web;
using System.Text.Json.Serialization;
using System.Configuration;
using SQLServerCourse.DAL.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters
                .Add(new JsonStringEnumConverter()));

var courseDBConnectionString = builder.Configuration.GetConnectionString("CourseDbConnection");
var filmDBConnectionString = builder.Configuration.GetConnectionString("FilmDbConnection");

builder.Services.AddDbContext<CourseDbContext>(options =>
    options.UseSqlServer(courseDBConnectionString));

builder.Services.AddDbContext<FilmDbContext>(options =>
    options.UseMySql(filmDBConnectionString,
            new MySqlServerVersion(new Version(8, 0, 25))));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Account/Login");
        options.AccessDeniedPath = new PathString("/Account/Login");
    });

builder.Services.AddRazorPages();

builder.Services.InitializeRepositories();
builder.Services.InitializeServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


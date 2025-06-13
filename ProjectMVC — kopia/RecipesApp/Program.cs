using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecipesApp.Data;
using RecipesApp.Models;
using System.Text;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

builder.Services.AddDbContext<RecipesContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("RecipesContext")
));



builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

builder.Services.AddHttpClient("Fdc", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Fdc:BaseUrl"]!);
});



// Add services to the container.

builder.Services.AddControllersWithViews();


var app = builder.Build();

app.UseStaticFiles();    // serve wwwroot/css/js/bootstrap, etc.
app.UseRouting();




app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Recipes}/{action=Index}/{id?}"
);


app.MapControllers();

app.Run();

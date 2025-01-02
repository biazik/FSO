using FSO.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var cultureInfo = new CultureInfo("pl-PL")
{
    NumberFormat = new NumberFormatInfo
    {
        NumberDecimalSeparator = "."
    },
    DateTimeFormat = new DateTimeFormatInfo
    {
        ShortDatePattern = "dd-MM-yyyy",
        LongDatePattern = "dd-MM-yyyy HH:mm:ss",
        ShortTimePattern = "HH:mm",
        LongTimePattern = "HH:mm:ss"
    }
};

// Ustawienie kultury globalnej
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using(var Scope = app.Services.CreateScope())
{
    var roleManager = Scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

}

using (var Scope = app.Services.CreateScope())
{
    var userManager = Scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string email = "admin@osiedlefso.pl";
    string pwd = "ZAQ1@wsx";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser();
        user.Email = email;
        user.UserName = email;
        user.EmailConfirmed = true;

        await userManager.CreateAsync (user, pwd);

        await userManager.AddToRoleAsync(user, "Admin");
    }

}

app.Run();

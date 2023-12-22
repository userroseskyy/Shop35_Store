using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop35_Store.Application.Helpers;
using Shop35_Store.Domain.Entities.Roles;

using Shop35_Store.Domain.Entities.Users;
using Shop35_Store.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var ConnectionString = builder.Configuration.GetConnectionString("DefaulConnectionString");
builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseSqlServer(ConnectionString));

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<DataBaseContext>().AddDefaultTokenProviders().AddErrorDescriber<CustomIdentityErrors>()
    .AddPasswordValidator<MyPasswordValidator>();

builder.Services.Configure<IdentityOptions>(options =>
{
    //User Setting
    options.User.RequireUniqueEmail = true;

    //Password
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
   



    //LocOut Setting
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);

    //signIn setting
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

});

builder.Services.ConfigureApplicationCookie(options =>
{
    //cookie setting
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);

    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;

});

builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, AddMyClaims>();
   


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(

    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

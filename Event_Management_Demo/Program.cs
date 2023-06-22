using Event_Management.Entities.Context;
using Event_Management.Repository;
using Event_Management.Repository.Interface;
using EventServices;
using EventServices.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.SignIn.RequireConfirmedEmail = true;
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IEventsPageRepository, EventsPageRepository>();
builder.Services.AddScoped<IEmailHelper, EmailHelper>();
builder.Services.AddScoped<INominatimLocation, NominatimLocation>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.ExpireTimeSpan = TimeSpan.FromMinutes(60 * 1);
    option.LoginPath = "/Account/Login";
    option.AccessDeniedPath = "/Account/Login";
});
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(1);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});



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

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Registration}/");

app.Run();

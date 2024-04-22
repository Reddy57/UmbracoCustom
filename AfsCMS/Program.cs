using ApplicationCore.Contracts.Services;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Umbraco
builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .Build();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IAccountService, AccountService>();

// Add database context for Identity
builder.Services.AddDbContext<AfsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("umbracoDbDSN")));

//sets the default authentication scheme for the app
/*
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "AfsScheme";
        options.DefaultAuthenticateScheme = "AfsScheme";
        options.DefaultChallengeScheme = "AfsScheme";
        options.DefaultSignInScheme = "AfsScheme";
    })
    .AddCookie("AfsScheme", options =>
    {
        options.Cookie.Name = "AfsAuthCookie";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });
    */


var app = builder.Build();

// Boot Umbraco
await app.BootUmbracoAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


// Umbraco middleware and endpoints
app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        //u.EndpointRouteBuilder.MapControllerRoute("OrdersController","/orders/{action}/{id?}",new {Controller="Orders",Action="Index"});
        u.UseInstallerEndpoints();
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

app.UseAuthentication(); // Ensure this is before UseAuthorization
app.UseAuthorization();

// MVC routes
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        "default",
        "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
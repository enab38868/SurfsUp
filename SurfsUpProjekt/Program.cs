using LazZiya.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SurfsUpProjekt.Data;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using SurfsUpProjekt.Models;
using MvcMovie.Models;
using SurfsUpProjekt.Core;
using Microsoft.AspNetCore.Authorization;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SurfsUpProjektContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SurfsUpProjektContext") ?? throw new InvalidOperationException("Connection string 'SurfsUpProjektContext' not found.")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() // Authorization??
    .AddEntityFrameworkStores<SurfsUpProjektContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<ITagHelperComponent, LocalizationValidationScriptsTagHelperComponent>();

var cultureInfo = new CultureInfo("da-DK");
cultureInfo.NumberFormat.CurrencyDecimalSeparator = ",";

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;


AddAuthorizationPolicies(); //AUTHORIZATION

var app = builder.Build();


using (var scope = app.Services.CreateScope()) //  ----- SEED DATABASE -----
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
    await SeedData.InitializeRoles(services);
}

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Rents}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

void AddAuthorizationPolicies()
{
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(ConstantsRole.Policies.RequireAdmin, policy => policy.RequireRole(ConstantsRole.Roles.Administrator));
        options.AddPolicy(ConstantsRole.Policies.RequireManager, policy => policy.RequireRole(ConstantsRole.Roles.Manager));

    });
}

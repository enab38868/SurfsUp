using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SurfsUpBlazor;
using SurfsUpBlazor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddDbContext<SurfsUpBlazorContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SurfsUpBlazorContext") ?? throw new InvalidOperationException("Connection string 'SurfsUpBlazorContext' not found.")));

builder.Services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() // Authorization??
    .AddEntityFrameworkStores<SurfsUpBlazorContext>();
//// Google Login halløj


//builder.Services.AddAuthentication().AddGoogle(googleOptions =>
//{
//    googleOptions.ClientId = "811708657220-j5sfn5tf5r76hjct9etd7mbuuejmr23o.apps.googleusercontent.com";
//    googleOptions.ClientSecret = "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";
//});
//builder.Services.AddAuthentication().AddFacebook(facebookoptions =>
//{
//    facebookoptions.AppId = "778589450131162";
//    facebookoptions.AppSecret = "97356bda0d4d79c7c29a37515cd5a691";
//});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddOidcAuthentication(options =>
{
    // Configure your authentication provider options here.
    // For more information, see https://aka.ms/blazor-standalone-auth
    builder.Configuration.Bind("Local", options.ProviderOptions);
});

await builder.Build().RunAsync();

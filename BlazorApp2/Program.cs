using BlazorApp2;
using BlazorApp2.Identity;
using BlazorApp2.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddScoped(
    sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddTransient<BearerTokenHandler>();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Eghatha",
    client =>client.BaseAddress = new Uri("https://localhost:7244")).AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddBlazoredLocalStorage();


builder.Services.AddScoped<ServiceApi>();
builder.Services.AddScoped<AdminHubService>();
builder.Services.AddScoped<TeamHubService>();
builder.Services.AddScoped<NotificationState>();

await builder.Build().RunAsync();

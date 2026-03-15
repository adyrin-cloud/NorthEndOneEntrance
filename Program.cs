using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NorthEndOneEntrance;
using NorthEndOneEntrance.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register employee check-in service as singleton to maintain state
builder.Services.AddSingleton<IEmployeeCheckInService, MockEmployeeCheckInService>();

// Register security service
builder.Services.AddSingleton<ISecurityService, MockSecurityService>();

// Register app state service to manage authentication and branch selection
builder.Services.AddSingleton<AppStateService>();

// Register branch service with dedicated HttpClient for external API calls
builder.Services.AddScoped<IBranchService>(sp =>
{
    var httpClient = new HttpClient();
    return new MockBranchService(httpClient);
});

await builder.Build().RunAsync();

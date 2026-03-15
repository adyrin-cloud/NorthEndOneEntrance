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

await builder.Build().RunAsync();

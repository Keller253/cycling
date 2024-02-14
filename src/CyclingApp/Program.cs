using Blazored.LocalStorage;
using CyclingApp.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace CyclingApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        // MudBlazor
        _ = builder.Services.AddMudServices();

        // Geolocation Api
        _ = builder.Services.AddGeolocationServices();

        // Screen Wake Lock Api
        _ = builder.Services.AddScreenWakeLockServices();

        // Local Storage Api
        _ = builder.Services.AddBlazoredLocalStorage();

        // Services
        _ = builder.Services.AddTransient<IActivityService, ActivityService>();

        // HttpClient
        _ = builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        await builder.Build().RunAsync();
    }
}

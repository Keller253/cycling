using Thinktecture.Blazor.ScreenWakeLock;

namespace CyclingApp
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="IScreenWakeLockService" /> service to the service collection.
        /// </summary>
        public static IServiceCollection AddScreenWakeLockServices(this IServiceCollection services)
        {
            ScreenWakeLockServiceCollectionExtensions.AddScreenWakeLockService(services);

            return services;
        }
    }
}

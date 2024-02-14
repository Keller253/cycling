using System.Runtime.CompilerServices;
using Blazored.LocalStorage;
using CyclingApp.DTOs;
using CyclingApp.Models;

namespace CyclingApp.Services;

internal class ActivityService : IActivityService
{
    private readonly ILocalStorageService _localStorageService;

    public ActivityService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;

        //_activities = [
        //new Activity(Guid.NewGuid(), DateTime.UtcNow.AddDays(-7), TimeSpan.FromMinutes(4), new ReadOnlyRoute(new List<GeoLocation>
        //{
        //    new(DateTime.UtcNow, 13.376842926876462, 52.518598063383926, 1, 100, 1, 45, 12),
        //    new(DateTime.UtcNow.AddMinutes(1), 13.37771344544642,52.51628190312695 , 1, 100, 1, 45, 12),
        //    new(DateTime.UtcNow.AddMinutes(2), 13.400707818550298, 52.51815134482483, 1, 100, 1, 45, 12),
        //})),
        //new Activity(Guid.NewGuid(), DateTime.UtcNow.AddDays(-6), TimeSpan.FromMinutes(4), new ReadOnlyRoute(new List<GeoLocation>
        //{
        //    new(DateTime.UtcNow, 13.376842926876462, 52.518598063383926, 1, 100, 1, 45, 12),
        //    new(DateTime.UtcNow.AddMinutes(1), 13.37771344544642,52.51628190312695 , 1, 100, 1, 45, 12),
        //    new(DateTime.UtcNow.AddMinutes(2), 13.400707818550298, 52.51815134482483, 1, 100, 1, 45, 12),
        //})),
        //new Activity(Guid.NewGuid(), DateTime.UtcNow.AddDays(-5), TimeSpan.FromMinutes(4), new ReadOnlyRoute(new List<GeoLocation>
        //{
        //    new(DateTime.UtcNow, 13.376842926876462, 52.518598063383926, 1, 100, 1, 45, 12),
        //    new(DateTime.UtcNow.AddMinutes(1), 13.37771344544642,52.51628190312695 , 1, 100, 1, 45, 12),
        //    new(DateTime.UtcNow.AddMinutes(2), 13.400707818550298, 52.51815134482483, 1, 100, 1, 45, 12),
        //})),
        //new Activity(Guid.NewGuid(), DateTime.UtcNow.AddDays(-4), TimeSpan.FromMinutes(4), new ReadOnlyRoute(new List<GeoLocation>
        //{
        //    new(DateTime.UtcNow, 13.376842926876462, 52.518598063383926, 1, 100, 1, 45, 12),
        //    new(DateTime.UtcNow.AddMinutes(1), 13.37771344544642,52.51628190312695 , 1, 100, 1, 45, 12),
        //    new(DateTime.UtcNow.AddMinutes(2), 13.400707818550298, 52.51815134482483, 1, 100, 1, 45, 12),
        //})),
        //new Activity(Guid.NewGuid(), DateTime.UtcNow.AddDays(-3), TimeSpan.FromMinutes(4), new ReadOnlyRoute(new List<GeoLocation>
        //{
        //    new(DateTime.UtcNow, 13.376842926876462, 52.518598063383926, 1, 100, 1, 45, 12),
        //    new(DateTime.UtcNow.AddMinutes(1), 13.37771344544642,52.51628190312695 , 1, 100, 1, 45, 12),
        //    new(DateTime.UtcNow.AddMinutes(2), 13.400707818550298, 52.51815134482483, 1, 100, 1, 45, 12),
        //})),
        //new Activity(Guid.NewGuid(), DateTime.UtcNow, TimeSpan.FromMinutes(4), new ReadOnlyRoute(new List<GeoLocation>
        //{
        //    new(DateTime.UtcNow, 13.376842926876462, 52.518598063383926, 1, 100, 1, 45, 12),
        //    new(DateTime.UtcNow.AddMinutes(1), 13.37771344544642,52.51628190312695 , 1, 100, 1, 45, 12),
        //    new(DateTime.UtcNow.AddMinutes(2), 13.400707818550298, 52.51815134482483, 1, 100, 1, 45, 12),
        //}))
        //];
        //Activities = new(_activities);
    }

    public async IAsyncEnumerable<Activity> GetActivitiesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var keys = await _localStorageService.KeysAsync(cancellationToken);
        foreach (var key in keys)
        {
            yield return await LoadActivityAsync(key, cancellationToken);
        }
    }

    public async Task<Activity> GetActivityAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var key = id.ToString();
        return await LoadActivityAsync(key, cancellationToken);
    }

    public async Task<Activity> CreateActivityAsync(DateTime creationTIme, TimeSpan duration, ReadOnlyRoute route,
                                                    CancellationToken cancellationToken = default)
    {
        var activity = new Activity(Guid.NewGuid(), creationTIme, duration, route);

        // Store in local storage
        await StoreActivityAsync(activity, cancellationToken);

        return activity;
    }

    public async Task DeleteActivityAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Remove from local storage
        await _localStorageService.RemoveItemAsync(id.ToString(), cancellationToken);
    }

    /// <exception cref="KeyNotFoundException">Thrown if the specified key was not found.</exception>
    private async Task<Activity> LoadActivityAsync(string key, CancellationToken cancellationToken = default)
    {
        var dto = await _localStorageService.GetItemAsync<StoreActivityDto>(key, cancellationToken);
        if (dto is null)
        {
            throw new KeyNotFoundException($"Specified key `{key}` not found");
        }
        var id = Guid.Parse(key);
        return dto.ToModel(id);
    }

    private async Task StoreActivityAsync(Activity activity, CancellationToken cancellationToken = default)
    {
        var dto = activity.ToDto();
        await _localStorageService.SetItemAsync(activity.Id.ToString(), dto, cancellationToken);
    }
}

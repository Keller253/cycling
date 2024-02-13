using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using CyclingApp.Models;

namespace CyclingApp.Services;

internal class ActivityService : IActivityService
{
    public ReadOnlyObservableCollection<Activity> Activities { get; }
    private readonly ObservableCollection<Activity> _activities;

    public ActivityService()
    {
        _activities = [
            new Activity(Guid.NewGuid(), DateTime.UtcNow.AddDays(-7), TimeSpan.FromMinutes(4), new ReadOnlyRoute(new List<GeoLocation>
            {
                new(DateTime.UtcNow, 13.376842926876462, 52.518598063383926, 1, 100, 1, 45, 12),
                new(DateTime.UtcNow.AddMinutes(1), 13.37771344544642,52.51628190312695 , 1, 100, 1, 45, 12),
                new(DateTime.UtcNow.AddMinutes(2), 13.400707818550298, 52.51815134482483, 1, 100, 1, 45, 12),
            })),
            new Activity(Guid.NewGuid(), DateTime.UtcNow.AddDays(-6), TimeSpan.FromMinutes(4), new ReadOnlyRoute(new List<GeoLocation>
            {
                new(DateTime.UtcNow, 13.376842926876462, 52.518598063383926, 1, 100, 1, 45, 12),
                new(DateTime.UtcNow.AddMinutes(1), 13.37771344544642,52.51628190312695 , 1, 100, 1, 45, 12),
                new(DateTime.UtcNow.AddMinutes(2), 13.400707818550298, 52.51815134482483, 1, 100, 1, 45, 12),
            })),
            new Activity(Guid.NewGuid(), DateTime.UtcNow.AddDays(-5), TimeSpan.FromMinutes(4), new ReadOnlyRoute(new List<GeoLocation>
            {
                new(DateTime.UtcNow, 13.376842926876462, 52.518598063383926, 1, 100, 1, 45, 12),
                new(DateTime.UtcNow.AddMinutes(1), 13.37771344544642,52.51628190312695 , 1, 100, 1, 45, 12),
                new(DateTime.UtcNow.AddMinutes(2), 13.400707818550298, 52.51815134482483, 1, 100, 1, 45, 12),
            })),
            new Activity(Guid.NewGuid(), DateTime.UtcNow.AddDays(-4), TimeSpan.FromMinutes(4), new ReadOnlyRoute(new List<GeoLocation>
            {
                new(DateTime.UtcNow, 13.376842926876462, 52.518598063383926, 1, 100, 1, 45, 12),
                new(DateTime.UtcNow.AddMinutes(1), 13.37771344544642,52.51628190312695 , 1, 100, 1, 45, 12),
                new(DateTime.UtcNow.AddMinutes(2), 13.400707818550298, 52.51815134482483, 1, 100, 1, 45, 12),
            })),
            new Activity(Guid.NewGuid(), DateTime.UtcNow.AddDays(-3), TimeSpan.FromMinutes(4), new ReadOnlyRoute(new List<GeoLocation>
            {
                new(DateTime.UtcNow, 13.376842926876462, 52.518598063383926, 1, 100, 1, 45, 12),
                new(DateTime.UtcNow.AddMinutes(1), 13.37771344544642,52.51628190312695 , 1, 100, 1, 45, 12),
                new(DateTime.UtcNow.AddMinutes(2), 13.400707818550298, 52.51815134482483, 1, 100, 1, 45, 12),
            })),
            new Activity(Guid.NewGuid(), DateTime.UtcNow, TimeSpan.FromMinutes(4), new ReadOnlyRoute(new List<GeoLocation>
            {
                new(DateTime.UtcNow, 13.376842926876462, 52.518598063383926, 1, 100, 1, 45, 12),
                new(DateTime.UtcNow.AddMinutes(1), 13.37771344544642,52.51628190312695 , 1, 100, 1, 45, 12),
                new(DateTime.UtcNow.AddMinutes(2), 13.400707818550298, 52.51815134482483, 1, 100, 1, 45, 12),
            }))
        ];
        Activities = new(_activities);
    }

    public Activity CreateActivity(DateTime creationTIme, TimeSpan duration, ReadOnlyRoute route)
    {
        var activity = new Activity(Guid.NewGuid(), creationTIme, duration, route);
        _activities.Add(activity);
        return activity;
    }

    public bool TryDeleteActivity(Guid id, [NotNullWhen(true)] out Activity? activity)
    {
        activity = Activities.FirstOrDefault(x => x.Id == id);
        return activity is not null
                && _activities.Remove(activity);
    }
}

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

using CyclingApp.Models;

namespace CyclingApp.Services
{
    internal class ActivityService : IActivityService
    {
        public ReadOnlyObservableCollection<Activity> Activities { get; }
        private readonly ObservableCollection<Activity> _activities;

        public ActivityService()
        {
            _activities = [];
            Activities = new(_activities);
        }

        public Activity CreateActivity(DateTime timestamp, TimeSpan duration, ReadOnlyRoute route)
        {
            var activity = new Activity(Guid.NewGuid(), timestamp, duration, route);
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
}

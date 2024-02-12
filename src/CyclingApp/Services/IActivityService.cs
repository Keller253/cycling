using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

using CyclingApp.Models;

namespace CyclingApp.Services
{
    public interface IActivityService
    {
        ReadOnlyObservableCollection<Activity> Activities { get; }

        Activity CreateActivity(DateTime timestamp, TimeSpan duration, ReadOnlyRoute route);

        bool TryDeleteActivity(Guid id, [NotNullWhen(true)] out Activity? activity);
    }
}
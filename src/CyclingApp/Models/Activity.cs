namespace CyclingApp.Models
{
    public class Activity
    {
        public Guid Id { get; }

        /// <summary>
        /// Creation time of the acitivity.
        /// </summary>
        public DateTime CreationTime { get; }

        /// <summary>
        /// Duration of the acitivity.
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Route of the activity.
        /// </summary>
        public ReadOnlyRoute Route { get; }

        /// <summary>
        /// Average speed during the activity in kilometers per hour.
        /// </summary>
        public double AvgSpeed { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="Activity"/> class.
        /// </summary>
        public Activity(Guid id, DateTime creationTime, TimeSpan duration, ReadOnlyRoute route)
        {
            Id = id;
            CreationTime = creationTime;
            Duration = duration;
            Route = route;
        }
    }
}

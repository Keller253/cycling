using CyclingApp.Services;

using Microsoft.AspNetCore.Components;

namespace CyclingApp.Pages
{
    public partial class Activity
    {
        /// <summary>
        /// Route of the page.
        /// </summary>
        public const string Route = "/Activity";

        private Models.Activity? _activity;

        /// <summary>
        /// ID of the activity to show.
        /// </summary>
        [Parameter]
        public Guid Id { get; set; }

        /// <summary>
        /// Service to get an activity.
        /// </summary>
        [Inject]
        protected IActivityService ActivityService { get; set; } = default!;

        /// <inheritdoc/>
        protected override void OnParametersSet()
        {
            _activity = ActivityService.Activities.FirstOrDefault(x => x.Id == Id);
        }
    }
}

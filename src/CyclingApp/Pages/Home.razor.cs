using Microsoft.AspNetCore.Components;

namespace CyclingApp.Pages
{
    public partial class Home
    {
        /// <summary>
        /// Route of the page.
        /// </summary>
        public const string Route = "/";

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        private void NavigateToTracker()
        {
            NavigationManager.NavigateTo(Tracker.Route);
        }

        private void NavigateToActivity(Guid id)
        {
            NavigationManager.NavigateTo($"{Activity.Route}/{id}");
        }
    }
}

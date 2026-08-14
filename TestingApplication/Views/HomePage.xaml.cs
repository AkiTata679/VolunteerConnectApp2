using VolunteerConnect2.Services;
using VolunteerConnect2.Models;

namespace VolunteerConnect2.Views
{
    public partial class HomePage : ContentPage
    {
        private OpportunityService _opportunityService = new OpportunityService();

        public HomePage()
        {
            InitializeComponent();
            LoadFeaturedOpportunity();
            SetupNavigation();
        }

        private async void LoadFeaturedOpportunity()
        {
            // Wait for DB initialization if needed
            if (!DatabaseService.IsInitialized)
                await DatabaseService.InitializeAsync();

            var opportunities = await _opportunityService.GetAllAsync();
            if (opportunities.Count == 0)
                return;

            var featured = opportunities.First();

            FeaturedImage.Source = featured.ImageName;
            FeaturedTitle.Text = featured.Title;
            FeaturedCategory.Text = featured.Category;
            FeaturedDate.Text = featured.Date.ToString("dd MMM yyyy");
        }

        private void SetupNavigation()
        {
            BrowseButton.Clicked += async (s, e) =>
            {
                await Shell.Current.GoToAsync("//OpportunitiesPage");
            };

            RegistrationsButton.Clicked += async (s, e) =>
            {
                await Shell.Current.GoToAsync("//MyRegistrationPage");
            };

            PrivacyButton.Clicked += async (s, e) =>
            {
                await Shell.Current.GoToAsync("//PrivacyPage");
            };
        }
    }
}

using VolunteerConnect2.Models;
using VolunteerConnect2.Services;

namespace VolunteerConnect2.Views
{
    public partial class HomePage : ContentPage
    {
        private OpportunityService _opportunityService = new OpportunityService();
        private List<VolunteerOpportunity> _allOpportunities = new();

        public HomePage()
        {
            InitializeComponent();
            LoadHomePageData();
        }

        private async void LoadHomePageData()
        {
            if (!DatabaseService.IsInitialized)
                await DatabaseService.InitializeAsync();

            _allOpportunities = await _opportunityService.GetAllAsync();

            // Total opportunities
            TotalOpportunitiesLabel.Text = $"Total Opportunities: {_allOpportunities.Count}";

            // Featured opportunity
            var featured = _allOpportunities.FirstOrDefault();
            if (featured != null)
            {
                FeaturedImage.Source = featured.ImageName;
                FeaturedTitle.Text = featured.Title;
                FeaturedLocation.Text = $"Location: {featured.Location}";
                FeaturedPlaces.Text = $"Available Places: {featured.AvailablePlaces}";
                FeaturedButton.BindingContext = featured;
            }
        }

        private async void FeaturedButton_Clicked(object sender, EventArgs e)
        {
            var opportunity = (sender as Button)?.BindingContext as VolunteerOpportunity;
            if (opportunity != null)
            {
                await Shell.Current.GoToAsync($"OpportunityDetailsPage?opportunityId={opportunity.Id}");
            }
        }

        private async void BrowseOpportunities_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("OpportunitiesPage");
        }

        private async void MyRegistrations_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("MyRegistrationPage");
        }
    }
}

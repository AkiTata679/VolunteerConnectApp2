using VolunteerConnect2.Models;
using VolunteerConnect2.Services;

namespace VolunteerConnect2.Views
{
    [QueryProperty(nameof(OpportunityId), "opportunityId")]
    public partial class OpportunityDetailsPage : ContentPage
    {
        private OpportunityService _opportunityService = new OpportunityService();

        public int OpportunityId { get; set; }

        public OpportunityDetailsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!DatabaseService.IsInitialized)
                await DatabaseService.InitializeAsync();

            await LoadOpportunity();
        }

        private async Task LoadOpportunity()
        {
            var opportunity = await _opportunityService.GetByIdAsync(OpportunityId);

            if (opportunity == null)
            {
                await DisplayAlert("Error", "The selected opportunity could not be found.", "OK");
                await Shell.Current.GoToAsync("///OpportunitiesPage");
                return;
            }

            OpportunityImage.Source = opportunity.ImageName;
            TitleLabel.Text = opportunity.Title;
            CategoryLabel.Text = $"Category: {opportunity.Category}";
            DateLabel.Text = $"Date: {opportunity.Date:dd MMM yyyy}";
            TimeLabel.Text = $"Time: {opportunity.Time}";
            LocationLabel.Text = $"Location: {opportunity.Location}";
            DescriptionLabel.Text = opportunity.Description;
            RequirementsLabel.Text = opportunity.Requirements;
            PlacesLabel.Text = $"Available Places: {opportunity.AvailablePlaces}";

            RegisterButton.Clicked += async (s, e) =>
            {
                await Shell.Current.GoToAsync($"RegistrationPage?opportunityId={opportunity.Id}");
            };
        }
    }
}

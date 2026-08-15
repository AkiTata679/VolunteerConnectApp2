using VolunteerConnect2.Models;
using VolunteerConnect2.Services;

namespace VolunteerConnect2.Views
{
    public partial class OpportunitiesPage : ContentPage
    {
        private OpportunityService _opportunityService = new OpportunityService();
        private List<VolunteerOpportunity> _allOpportunities = new();

        public OpportunitiesPage()
        {
            InitializeComponent();
            LoadData();
            SetupEvents();
        }

        private async void LoadData()
        {
            if (!DatabaseService.IsInitialized)
                await DatabaseService.InitializeAsync();

            _allOpportunities = await _opportunityService.GetAllAsync();
            OpportunitiesList.ItemsSource = _allOpportunities;
        }

        private void SetupEvents()
        {
            SearchBar.TextChanged += (s, e) => ApplyFilters();
        }

        private void ApplyFilters()
        {
            IEnumerable<VolunteerOpportunity> filtered = _allOpportunities;

            // Search by title only
            if (!string.IsNullOrWhiteSpace(SearchBar.Text))
            {
                string query = SearchBar.Text.ToLower();
                filtered = filtered.Where(o => o.Title.ToLower().Contains(query));
            }

            OpportunitiesList.ItemsSource = filtered.ToList();
        }

        private async void ViewDetailsClicked(object sender, EventArgs e)
        {
            var opportunity = (sender as Button)?.BindingContext as VolunteerOpportunity;
            if (opportunity != null)
            {
                await Shell.Current.GoToAsync($"OpportunityDetailsPage?opportunityId={opportunity.Id}");
            }
        }
    }
}
